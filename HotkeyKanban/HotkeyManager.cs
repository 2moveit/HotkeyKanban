using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KCT.HotkeyKanban.HotkeyAPI;
using KCT.HotkeyKanban.Settings;

namespace KCT.HotkeyKanban
{
    public sealed class HotkeyManager
    {
        #region Singleton

        private static readonly HotkeyManager instance = new HotkeyManager();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static HotkeyManager()
        {
        }

        private HotkeyManager()
        {
        }

        public static HotkeyManager Instance
        {
            get { return instance; }
        }

        #endregion

        private Hotkey hotkey;
        private Window window;
        private Action<Window> action;
        public void RegisterWindow(Window hotkeyWindow, Action<Window> hotkeyAction)
        {
            window = hotkeyWindow;
            action = hotkeyAction;
            Register();
        }

        public void RegisterHotkey(ModifierKeys keyModifier, Keys hotkey)
        {
            HotkeySettings.Default.ModifierKey = keyModifier;
            HotkeySettings.Default.Key = hotkey;
            HotkeySettings.Default.Save();
            Register();
        }

        private void Register()
        {
            if(hotkey != null)
                hotkey.Dispose();
            hotkey = new Hotkey(ModifierKey, Key, window);
            hotkey.HotkeyPressed += (k) => action(window);
        }


        

        public ModifierKeys ModifierKey
        {
            get { return HotkeySettings.Default.ModifierKey; }
        }


        public Keys Key
        {
            get { return HotkeySettings.Default.Key; }
        }
    }

}
