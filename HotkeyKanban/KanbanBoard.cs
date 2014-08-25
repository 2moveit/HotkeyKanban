using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KCT.HotkeyKanban
{
    public class KanbanBoard : IKanbanBoard
    {
        private readonly Dictionary<DateTime, IEnumerable<Task>> archive = new Dictionary<DateTime, IEnumerable<Task>>();
        private readonly List<Task> board = new List<Task>();

        /// <summary>
        ///     Create new task in backlog
        /// </summary>
        /// <param name="taskDescription"></param>
        public void CreateTask(Guid taskId, string taskDescription)
        {
            board.Add(new Task(taskId, taskDescription));
        }

        /// <summary>
        ///     Move task to next
        /// </summary>
        /// <param name="taskId"></param>
        public void SheduleTask(Guid taskId)
        {
            board.Single(t => t.Id.Equals(taskId)).State = KanbanState.Sheduled;
        }

        /// <summary>
        ///     Move task to work in progress
        /// </summary>
        /// <param name="taskId"></param>
        public void BeginWorkOnTask(Guid taskId)
        {
            board.Single(t => t.Id.Equals(taskId)).State = KanbanState.WorkInProgress;
        }

        /// <summary>
        ///     Move task to waiting
        /// </summary>
        /// <param name="taskId"></param>
        public void InterruptTask(Guid taskId)
        {
            board.Single(t => t.Id.Equals(taskId)).State = KanbanState.Waiting;
        }

        /// <summary>
        ///     Moves task to work in progress
        /// </summary>
        /// <param name="taskId"></param>
        public void ContinueTask(Guid taskId)
        {
            board.Single(t => t.Id.Equals(taskId)).State = KanbanState.WorkInProgress;
        }

        /// <summary>
        ///     Move task to done
        /// </summary>
        /// <param name="taskId"></param>
        public void CloseTask(Guid taskId)
        {
            board.Single(t => t.Id.Equals(taskId)).State = KanbanState.Done;
        }

        /// <summary>
        ///     Move tasks from done to archive
        /// </summary>
        public void ArchiveTasks()
        {
            IList<Task> closedTasks = board.Where(t => t.State.Equals(KanbanState.Done)).ToList();
            board.RemoveAll(t => t.State.Equals(KanbanState.Done));

            foreach (Task closedTask in closedTasks)
            {
                closedTask.State = KanbanState.Archived;
            }
            archive.Add(DateTime.Now, closedTasks);
        }

        public IEnumerable<Task> GetBacklogTasks()
        {
            return board.Where(t => t.State.Equals(KanbanState.Backlog));
        }

        public IEnumerable<Task> GetSheduledTasks()
        {
            return board.Where(t => t.State.Equals(KanbanState.Sheduled));
        }

        public IEnumerable<Task> GetTasksInProgress()
        {
            return board.Where(t => t.State.Equals(KanbanState.WorkInProgress));
        }

        public IEnumerable<Task> GetWaitingTasks()
        {
            return board.Where(t => t.State.Equals(KanbanState.Waiting));
        }

        public IEnumerable<Task> GetClosedTasks()
        {
            return board.Where(t => t.State.Equals(KanbanState.Done));
        }

        public IDictionary<DateTime, IEnumerable<Task>> GetArchivedTasks()
        {
            return new Dictionary<DateTime, IEnumerable<Task>>(archive);
        }

        public void Load(FileInfo file)
        {
        }

        public void Save(FileInfo file)
        {
        }
    }
}