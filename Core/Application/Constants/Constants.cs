using static Application.Enums.Enums;

namespace Application.Constants;

public class Constants
{
    public static readonly IReadOnlyDictionary<EnumTaskStatus, Guid> TaskStatusDetailIds =
           new Dictionary<EnumTaskStatus, Guid>
           {
           { EnumTaskStatus.Pending,    new("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAA1") },
           { EnumTaskStatus.InProgress, new("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAA2") },
           { EnumTaskStatus.Done,       new("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAA3") },
           };

    public static readonly IReadOnlyDictionary<TaskPriority, Guid> TaskPriorityDetailIds =
        new Dictionary<TaskPriority, Guid>
        {
        { TaskPriority.High,   new("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBB1") },
        { TaskPriority.Medium, new("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBB2") },
        { TaskPriority.Low,    new("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBB3") },
        };
    public static readonly IReadOnlyDictionary<ValuesType, Guid> ValuesMasterIds =
       new Dictionary<ValuesType, Guid>
       {
            { ValuesType.TaskStatuses,   new("11111111-1111-1111-1111-111111111111") },
            { ValuesType.TaskPriorities, new("22222222-2222-2222-2222-222222222222") },
       };
}
