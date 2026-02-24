namespace Application.Enums;

public class Enums
{
    public enum ValuesType
    {
        TaskStatuses = 1,
        TaskPriorities = 2
    }
    public enum EnumTaskStatus
    {
        Pending = 1,
        InProgress = 2,
        Done = 3
    }

    public enum TaskPriority
    {
        High = 1,
        Medium = 2,
        Low = 3
    }
}