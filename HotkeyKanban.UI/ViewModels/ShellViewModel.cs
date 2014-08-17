using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;

namespace KCT.HotkeyKanban.UI.ViewModels
{
    public class ShellViewModel : Screen, IShell
    {
        protected override void OnActivate()
        {
            base.OnActivate(); //TODO: FOCUS TBX (IRESULT? FOCUSMANGER? )
          
        }

        private readonly IWindowManager windowManager;
        public ShellViewModel(IWindowManager windowManager)
        {
            this.windowManager = windowManager;
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

        string input;

        public string Input
        {
            get { return input; }
            set
            {
                input = value;
                NotifyOfPropertyChange(() => Input);
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
