namespace KCT.HotkeyKanban
{
    public enum KanbanState
    {
        Backlog,
        Sheduled,
        Waiting,
        WorkInProgress,
        Done,
        Archived
    }

    public static class KanbanStateExtensions
    {
        public static KanbanState GetNextState(this KanbanState currentState)
        {
            return (KanbanState) ((int) currentState + 1);
        }
    }
}