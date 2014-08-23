using Caliburn.Micro;
using KCT.HotkeyKanban.HotkeyAPI;

namespace KCT.HotkeyKanban.UI.ViewModels
{
    public sealed class OptionsViewModel : Screen
    {
        private Keys key;
        private ModifierKeys modifierKey;

        public OptionsViewModel()
        {
            DisplayName = "Options";
        }

        public ModifierKeys ModifierKey
        {
            get { return modifierKey; }
            set
            {
                if (modifierKey != value)
                {
                    modifierKey = value;
                    NotifyOfPropertyChange(() => ModifierKey);
                }
            }
        }

        public Keys Key
        {
            get { return key; }
            set
            {
                if (key != value)
                {
                    key = value;
                    NotifyOfPropertyChange(() => Key);
                }
            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            Key = HotkeyManager.Instance.Key;
            ModifierKey = HotkeyManager.Instance.ModifierKey;
        }

        public void Save()
        {
            HotkeyManager.Instance.RegisterHotkey(ModifierKey, Key);
            TryClose(true);
        }
    }
}