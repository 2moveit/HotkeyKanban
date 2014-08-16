using System.Windows;
using Caliburn.Micro;

namespace KCT.HotkeyKanban.UI
{
    public class AppWindowManager : WindowManager
    {
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            Window window = base.EnsureWindow(model, view, isDialog);
            //if app uses multiple windows then check here which to register, e.g. by checking an interface
            window.Loaded += (s, e) => HotkeyManager.RegisterWindow(window);
            return window;
        }
    }
}