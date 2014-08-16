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
    public static class HotkeyManager
    {
        public static void RegisterWindow(Window window)
        {
            var hotkey = new Hotkey(ModifierKeys.Alt, Keys.Space, window);
            hotkey.HotkeyPressed += (k) =>
            {  
                window.Activate();
                window.WindowState = WindowState.Normal;
                Console.Beep();
            };
        }
    }
}
