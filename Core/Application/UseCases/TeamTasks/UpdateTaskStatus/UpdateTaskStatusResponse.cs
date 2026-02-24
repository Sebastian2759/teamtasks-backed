using Application.Dtos;

namespace Application.UseCases.TeamTasks.UpdateTaskStatus;

public class UpdateTaskStatusResponse
{
    public TaskStatusUpdatedDto Task { get; set; } = default!;
}
