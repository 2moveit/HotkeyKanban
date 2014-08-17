using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;

namespace KCT.HotkeyKanban.UI
{
    public class AppWindowManager : WindowManager
    {
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            Window window = base.EnsureWindow(model, view, isDialog);
            window.ResizeMode = ResizeMode.NoResize;
            return window;
        }

        protected override Window CreateWindow(object rootModel, bool isDialog, object context, IDictionary<string, object> settings)
        {
            Window window = base.CreateWindow(rootModel, isDialog, context, settings);
            if (rootModel is IShell)
            {
                window.Loaded += (s, e) => HotkeyManager.RegisterWindow((Window) s);
            }
            return window;
        }
    }
}