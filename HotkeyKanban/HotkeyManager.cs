using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using KCT.HotkeyAPI;

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
            get
            {
                return instance;
            }
        }
        #endregion
        public void RegisterWindow(Window window, Action<Window> action)
        {
            var hotkey = new Hotkey(ModifierKeys.Alt, Keys.Space, window);
            hotkey.HotkeyPressed += (k) => action(window);
        }
    } 
}
