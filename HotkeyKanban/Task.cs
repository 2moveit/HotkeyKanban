using System;

namespace KCT.HotkeyKanban
{
    public class Task
    {
        public Task(Guid newGuid, string taskDescription)
        {
            Id = newGuid;
            Description = taskDescription;
            State = KanbanState.Backlog;
        }

        public Guid Id { get; set; }
        public string Description { get; set; }
        public KanbanState State { get; set; }
    }
}