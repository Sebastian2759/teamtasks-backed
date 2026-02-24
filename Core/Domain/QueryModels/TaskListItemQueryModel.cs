using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.QueryModels
{
    public class TaskListItemQueryModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;

        public Guid AssignedUserId { get; set; }
        public string AssignedUserName { get; set; } = default!;

        public string Status { get; set; } = default!; // Pending/InProgress/Done

        public Guid? PriorityId { get; set; }
        public string? Priority { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        // SP devuelve TotalCount por fila
        public long TotalCount { get; set; }
    }
}
