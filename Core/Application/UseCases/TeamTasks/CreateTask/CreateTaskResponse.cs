using Application.Dtos.TeamTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.TeamTasks.CreateTask
{
    public class CreateTaskResponse 
    {
        public TaskDto Task { get; set; } = new();
    }
}
