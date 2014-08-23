using System;

namespace KCT.HotkeyKanban.HotkeyAPI
{
    [Flags]
    public enum ModifierKeys
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8,
    }
}