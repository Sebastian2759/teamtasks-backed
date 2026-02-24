using Application.Base;
using MediatR;

namespace Application.UseCases.TeamTasks.CreateTask;

public class CreateTaskRequest : IRequest<ResponseBase<CreateTaskResponse>>
{
    public string Title {get;set;}
    public string? Description {get;set;}
    public Guid AssignedUserId {get;set;}
    public Guid? PriorityId { get; set; }
    public string? AdditionalInfo {get;set;}
}