using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCT.HotkeyKanban;
using Should;
using Xunit;
using Task = KCT.HotkeyKanban.Task;

namespace HotkeyKanban.tests
{
    public class KanbanBoardTests
    {
        const string TaskDescription = "testTask";
        Guid taskId = Guid.NewGuid();
        private readonly KanbanBoard sut;
        public KanbanBoardTests()
        {
            sut = new KanbanBoard(new PersistencyMockup());
        }
        [Fact]
        public void CreateTask_createsBacklogItem()
        {
            sut.CreateTask(taskId, TaskDescription);
            IEnumerable<Task> backlogTasks = sut.GetBacklogTasks();

            backlogTasks.First().Description.ShouldEqual(TaskDescription);
        }

        [Fact]
        public void SheduleTask_MovesItemFromBacklogToSheduledTasks()
        {
            sut.CreateTask(taskId, TaskDescription);
            IEnumerable<Task> initialBacklogTasks = sut.GetBacklogTasks();

            sut.SheduleTask(initialBacklogTasks.First().Id);
            IEnumerable<Task> sheduledTasks = sut.GetSheduledTasks();
            IEnumerable<Task> backlogTasks = sut.GetBacklogTasks();

            sheduledTasks.First().Description.ShouldEqual(TaskDescription);
            backlogTasks.Count().ShouldEqual(0);
        }
        [Fact]
        public void BeginWorkOnTask_MovesItemFromSheduledToWorkInProgress()
        {
            sut.CreateTask(taskId, TaskDescription);
            IEnumerable<Task> initialBacklogTasks = sut.GetBacklogTasks();
            var id = initialBacklogTasks.First().Id;
            sut.SheduleTask(id);

            sut.BeginWorkOnTask(id);
            IEnumerable<Task> tasksInProgress = sut.GetTasksInProgress();
            IEnumerable<Task> sheduledTasks = sut.GetSheduledTasks();

            tasksInProgress.First().Description.ShouldEqual(TaskDescription);
            sheduledTasks.Count().ShouldEqual(0);
        }



        [Fact]
        public void CloseTask_MovesItemFromWorkInProgressToDone()
        {
            sut.CreateTask(taskId, TaskDescription);
            IEnumerable<Task> initialBacklogTasks = sut.GetBacklogTasks();
            var id = initialBacklogTasks.First().Id;
            sut.SheduleTask(id);
            sut.BeginWorkOnTask(id);

            sut.CloseTask(id);

            IEnumerable<Task> closedTasks = sut.GetClosedTasks();
            IEnumerable<Task> tasksInProgress = sut.GetTasksInProgress();

            closedTasks.First().Description.ShouldEqual(TaskDescription);
            tasksInProgress.Count().ShouldEqual(0);
        }

        [Fact]
        public void InteruptTask_MovesItemFromWorkInProgressToWaiting()
        {
            sut.CreateTask(taskId, TaskDescription);
            IEnumerable<Task> initialBacklogTasks = sut.GetBacklogTasks();
            var id = initialBacklogTasks.First().Id;
            sut.SheduleTask(id);
            sut.BeginWorkOnTask(id);

            sut.InterruptTask(id);

            IEnumerable<Task> waitingTasks = sut.GetWaitingTasks();
            IEnumerable<Task> tasksInProgress = sut.GetTasksInProgress();

            waitingTasks.First().Description.ShouldEqual(TaskDescription);
            tasksInProgress.Count().ShouldEqual(0);
        }

        [Fact]
        public void ContinueTask_MovesItemFromWaitingToWorkInProgress()
        {
            sut.CreateTask(taskId, TaskDescription);
            IEnumerable<Task> initialBacklogTasks = sut.GetBacklogTasks();
            var id = initialBacklogTasks.First().Id;
            sut.SheduleTask(id);
            sut.BeginWorkOnTask(id);
            sut.InterruptTask(id);

            sut.ContinueTask(id);

            IEnumerable<Task> tasksInProgress = sut.GetTasksInProgress();
            IEnumerable<Task> waitingTasks = sut.GetWaitingTasks();
            

            tasksInProgress.First().Description.ShouldEqual(TaskDescription);
            waitingTasks.Count().ShouldEqual(0);
        }

        [Fact]
        public void ArchiveTask_MovesItemFromClosedToArchive()
        {
            sut.CreateTask(taskId, TaskDescription);
            IEnumerable<Task> initialBacklogTasks = sut.GetBacklogTasks();
            var id = initialBacklogTasks.First().Id;
            sut.SheduleTask(id);
            sut.BeginWorkOnTask(id);
            sut.CloseTask(id);

            sut.ArchiveTasks();

            IDictionary<DateTime, IEnumerable<Task>> archivedTasks = sut.GetArchivedTasks();
            IEnumerable<Task> closedTasks = sut.GetClosedTasks();

            archivedTasks[archivedTasks.Keys.First()].First().Description.ShouldEqual(TaskDescription);
            closedTasks.Count().ShouldEqual(0);
        }
    }
}
