﻿using System;
using System.Runtime.InteropServices;

namespace KCT.HotkeyKanban.HotkeyAPI
{
    internal class HotkeyWinApi
    {
        public const int WmHotkey = 0x0312;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, ModifierKeys fsModifiers, Keys vk);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}