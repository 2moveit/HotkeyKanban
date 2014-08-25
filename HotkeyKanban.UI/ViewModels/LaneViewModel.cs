using System.Collections.ObjectModel;
using System.ComponentModel;
using Caliburn.Micro;

namespace KCT.HotkeyKanban.UI.ViewModels
{
    public class LaneViewModel :Screen
    {
       

        public LaneViewModel(KanbanState state)
        {
            State = state;
            Cards = new ObservableCollection<CardViewModel>();
        }

        public KanbanState State { get; set; }

        private ObservableCollection<CardViewModel> cards;
        public ObservableCollection<CardViewModel> Cards
        {
            get { return cards; }
            set
            {
                if (Equals(value, cards)) return;
                cards = value;
                NotifyOfPropertyChange(() => Cards);
            }
        }
    }
}