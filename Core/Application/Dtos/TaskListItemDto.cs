using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public sealed class TaskListItemDto
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;

        public Guid AssignedUserId { get; init; }
        public string AssignedUserName { get; init; } = string.Empty;

        public string Status { get; init; } = string.Empty;   // Pending/InProgress/Done

        public Guid? PriorityId { get; init; }
        public string? Priority { get; init; }

        public DateTime CreatedAtUtc { get; init; }
    }
}
