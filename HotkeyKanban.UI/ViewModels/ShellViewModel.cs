using System;
using System.Collections.Generic;
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
            InitLane(KanbanState.Backlog);
            InitLane(KanbanState.Sheduled);
            InitLane(KanbanState.Waiting);
            InitLane(KanbanState.WorkInProgress);
            InitLane(KanbanState.Done);
        }

        private void InitLane(KanbanState state)
        {
            Lanes.Add(new LaneViewModel(state));
            LoadTasks(state);
        }

        private void LoadTasks(KanbanState state)
        {
            IList<Task> tasks = board.GetTasks(state).ToList();
            var cards = new ObservableCollection<CardViewModel>();
            for (int index = 0; index < tasks.Count; index++)
            {
                cards.Add(new CardViewModel(tasks[index], CreateShortId(state, index)));
            }
            Lanes[(int)state].Cards = cards;
        }

        private int CreateShortId(KanbanState state, int cardIndex)
        {
            string stateString = ((int) state).ToString();
            string cardIndexString = (cardIndex + 1).ToString();
            return int.Parse(stateString + cardIndexString);
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
            var taskId = Guid.NewGuid();
            board.CreateTask(taskId, Input);
            LoadTasks(KanbanState.Backlog);
            //Task task = board.GetTask(taskId);
            //Lanes.Single(l => l.State == KanbanState.Backlog).Cards.Add(new CardViewModel(task));
            NotifyOfPropertyChange(() => Lanes);

            board.Save(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Kanban.board")));
        }
    }
}
