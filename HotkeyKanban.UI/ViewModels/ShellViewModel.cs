using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using KCT.HotkeyKanban.UI.Views;

namespace KCT.HotkeyKanban.UI.ViewModels
{
    public sealed class ShellViewModel : Screen, IShell, IHandle<HotkeyPressed>
    {
        private readonly IWindowManager windowManager;
        private readonly IEventAggregator eventAggregator;

        public ShellViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            DisplayName = "Hotkey Kanban";
        }

        public void ExecuteAddTask(Key key)
        {
            if (key == Key.Enter && Input != string.Empty)
            {
                AddTask();
                Input = string.Empty;
            }
        }

        public void Handle(HotkeyPressed e)
        {

            InputIsFocused = true;

        }

        private bool inputIsFocused;
        public bool InputIsFocused
        {
            get { return inputIsFocused; }
            set
            {
                if (inputIsFocused != value)
                {
                    inputIsFocused = value;
                    NotifyOfPropertyChange(() => InputIsFocused);
                }
            }
        }

        string input;

        public string Input
        {
            get { return input; }
            set
            {
                if (input != value)
                {
                    input = value;
                    NotifyOfPropertyChange(() => Input);
                }
            }
        }


        public void OpenOptions()
        {
           windowManager.ShowDialog(new OptionsViewModel());
        }

        public void AddTask()
        {
            MessageBox.Show(string.Format("Added task: {0}", Input));
        }
    }
}
