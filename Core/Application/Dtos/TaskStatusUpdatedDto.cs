using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class TaskStatusUpdatedDto
    {
        public Guid Id { get; set; }
        public Guid StatusId { get; set; }
        public string Status { get; set; } = default!;
        public DateTime UpdatedAtUtc { get; set; }
    }
}
