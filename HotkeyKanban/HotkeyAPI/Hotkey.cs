using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Interop;

namespace KCT.HotkeyKanban.HotkeyAPI
{
    public sealed class Hotkey : IDisposable
    {
        private readonly IntPtr handle;
        private readonly int id;
        private bool disposed;
        private bool isKeyRegistered;

        public Hotkey(ModifierKeys modifierKeys, Keys key, Window window)
            : this(modifierKeys, key, new WindowInteropHelper(window))
        {
            Contract.Assert(window != null, "Window cannot be null.");
        }

        public Hotkey(ModifierKeys modifierKeys, Keys key, WindowInteropHelper window)
            : this(modifierKeys, key, window.Handle)
        {
            Contract.Assert(window != null, "Window cannot be null.");
        }

        public Hotkey(ModifierKeys modifierKeys, Keys key, IntPtr windowHandle)
        {
            Contract.Assert(modifierKeys != ModifierKeys.None || key != Keys.None,
                "At least a modifier key or a key is required.");
            Contract.Assert(windowHandle != IntPtr.Zero, "Windows handle cannot be 0");

            Key = key;
            KeyModifier = modifierKeys;
            id = GetHashCode();
            handle = windowHandle;
            RegisterHotKey();
            ComponentDispatcher.ThreadPreprocessMessage += ThreadPreprocessMessageMethod;
        }

        public Keys Key { get; private set; }

        public ModifierKeys KeyModifier { get; private set; }

        public void Dispose()
        {
            Dispose(true);
        }

        public event Action<Hotkey> HotkeyPressed;

        ~Hotkey()
        {
            Dispose(false);
        }

        public void RegisterHotKey()
        {
            if (Key == Keys.None)
                return;
            if (isKeyRegistered)
                UnregisterHotKey();
            isKeyRegistered = HotkeyWinApi.RegisterHotKey(handle, id, KeyModifier, Key);
            if (!isKeyRegistered)
                throw new HotkeyAlreadyInUseException();
        }

        public void UnregisterHotKey()
        {
            isKeyRegistered = !HotkeyWinApi.UnregisterHotKey(handle, id);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    ComponentDispatcher.ThreadPreprocessMessage -= ThreadPreprocessMessageMethod;
                }

                UnregisterHotKey();
                disposed = true;
            }
        }

        private void ThreadPreprocessMessageMethod(ref MSG msg, ref bool handled)
        {
            if (!handled)
            {
                if (msg.message == HotkeyWinApi.WmHotkey
                    && (int) (msg.wParam) == id)
                {
                    OnHotKeyPressed();
                    handled = true;
                }
            }
        }

        private void OnHotKeyPressed()
        {
            if (HotkeyPressed != null)
                HotkeyPressed(this);
        }
    }
}