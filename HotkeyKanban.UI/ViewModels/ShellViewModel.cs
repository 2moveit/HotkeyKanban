using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;

namespace KCT.HotkeyKanban.UI.ViewModels
{
    public sealed class ShellViewModel : Screen, IShell, IHandle<HotkeyPressed>
    {
        private readonly IWindowManager windowManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IKanbanBoard board;
        public ShellViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IKanbanBoard board)
        {

            this.windowManager = windowManager;
            this.board = board;
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            DisplayName = "Hotkey Kanban";
            Lanes = new ObservableCollection<LaneViewModel>();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Lanes.Add(new LaneViewModel(KanbanState.Backlog));
            Lanes.Add(new LaneViewModel(KanbanState.Sheduled));
            Lanes.Add(new LaneViewModel(KanbanState.Waiting));
            Lanes.Add(new LaneViewModel(KanbanState.WorkInProgress));
            Lanes.Add(new LaneViewModel(KanbanState.Done));
        }


        private ObservableCollection<LaneViewModel> lanes;
        public ObservableCollection<LaneViewModel> Lanes
        {
            get { return lanes; }
            set
            {
                lanes = value;
                NotifyOfPropertyChange(() => Lanes);
            }
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
            board.CreateTask(Guid.NewGuid(), Input);
           //TODO: var task = board.GetTasks(taskId);
            Lanes.Single(l => l.State == KanbanState.Backlog).Cards.Add(new CardViewModel { Description = Input });
            NotifyOfPropertyChange(() => Lanes);
        }
    }
}
