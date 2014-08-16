using System.Windows;
using Caliburn.Micro;

namespace KCT.HotkeyKanban.UI.ViewModels
{
    public class ShellViewModel : PropertyChangedBase, IShell
    {
     
        string input;

        public string Input
        {
            get { return input; }
            set
            {
                input = value;
                NotifyOfPropertyChange(() => Input);
                NotifyOfPropertyChange(() => CanSayHello);
            }
        }

        public bool CanSayHello
        {
            get { return !string.IsNullOrWhiteSpace(Input); }
        }

        public void SayHello()
        {
            MessageBox.Show(string.Format("Hello {0}!", Input)); //Don't do this in real life :)
        }
    }
}
