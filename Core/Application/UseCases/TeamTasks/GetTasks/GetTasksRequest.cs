using Application.Base;
using MediatR;

namespace Application.UseCases.TeamTasks.GetTasks;

public class GetTasksRequest : IRequest<ResponseBase<GetTasksResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public Guid? AssignedUserId { get; set; }
    public string? Status { get; set; } 
    public Guid? PriorityId { get; set; }
    public string? Tag { get; set; }
    public string? Search { get; set; }
}
