using Application.Base;
using MediatR;

namespace Application.UseCases.TeamTasks.CreateTask;

public class CreateTaskRequest : IRequest<ResponseBase<CreateTaskResponse>>
{
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }

    public Guid AssigneeId { get; set; }
    public Guid IdTaskStatus { get; set; }
    public Guid IdTaskPriority { get; set; }

    public int EstimatedComplexity { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? CompletionDate { get; set; }
}