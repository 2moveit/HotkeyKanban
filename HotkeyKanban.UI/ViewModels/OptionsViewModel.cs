using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace KCT.HotkeyKanban.UI.ViewModels
{
    public sealed class OptionsViewModel : Screen
    {
        public OptionsViewModel()
        {
            DisplayName = "Options";
        }

        public string ModifierKey { get; set; }
        public string Key { get; set; }

        public void Save()
        {
            //Todo: Save settings, Reregister Hotkey
            TryClose(true);
        }

    }
}
