using Application.Dtos;

namespace Application.UseCases.TeamTasks.CreateTask
{
    public class CreateTaskResponse 
    {
        public TaskCreatedDto Task { get; set; } = default!;
    }
}
