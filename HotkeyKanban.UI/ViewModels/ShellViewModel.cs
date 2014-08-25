using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
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
            board.Load(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Kanban.board")));
            LoadTasks(KanbanState.Backlog);
            LoadTasks(KanbanState.Sheduled);
            LoadTasks(KanbanState.Waiting);
            LoadTasks(KanbanState.WorkInProgress);
            LoadTasks(KanbanState.Done);
        }

        private void LoadTasks(KanbanState state)
        {
            Lanes.Add(new LaneViewModel(state));
            foreach (Task task in board.GetTasks(state))
            {
                Lanes[(int)state].Cards.Add(new CardViewModel { Id = task.Id, Description = task.Description });
            }
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

            board.Save(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Kanban.board")));
        }
    }
}
