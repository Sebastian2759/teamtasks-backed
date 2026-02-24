using Application.Base;
using MediatR;

namespace Application.UseCases.TeamTasks.UpdateTaskStatus;

public class UpdateTaskStatusRequest : IRequest<ResponseBase<UpdateTaskStatusResponse>>
{
    public UpdateTaskStatusRequest(Guid taskId, string status)
    {
        TaskId = taskId;
        Status = status;
    }

    public Guid TaskId { get; set; }
    public string Status { get; set; } = default!;
}
