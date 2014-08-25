using System;
using System.Collections.Generic;
using System.IO;

namespace KCT.HotkeyKanban
{
    public interface IKanbanBoard
    {
        /// <summary>
        ///     Create new task in backlog
        /// </summary>
        /// <param name="taskDescription"></param>
        void CreateTask(Guid taskId, string taskDescription);

        /// <summary>
        ///     Move task to next
        /// </summary>
        /// <param name="taskId"></param>
        void SheduleTask(Guid taskId);

        /// <summary>
        ///     Move task to work in progress
        /// </summary>
        /// <param name="taskId"></param>
        void BeginWorkOnTask(Guid taskId);

        /// <summary>
        ///     Move task to waiting
        /// </summary>
        /// <param name="taskId"></param>
        void InterruptTask(Guid taskId);

        /// <summary>
        ///     Moves task to work in progress
        /// </summary>
        /// <param name="taskId"></param>
        void ContinueTask(Guid taskId);

        /// <summary>
        ///     Move task to done
        /// </summary>
        /// <param name="taskId"></param>
        void CloseTask(Guid taskId);

        /// <summary>
        ///     Move tasks from done to archive
        /// </summary>
        void ArchiveTasks();

        IEnumerable<Task> GetBacklogTasks();
        IEnumerable<Task> GetSheduledTasks();
        IEnumerable<Task> GetTasksInProgress();
        IEnumerable<Task> GetWaitingTasks();
        IEnumerable<Task> GetClosedTasks();
        IDictionary<DateTime, IEnumerable<Task>> GetArchivedTasks();
        void Load(FileInfo file);
        void Save(FileInfo file);
        IEnumerable<Task> GetTasks(KanbanState state);
    }
}