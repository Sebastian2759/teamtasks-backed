using Application.Base;
using MediatR;

namespace Application.UseCases.TeamTasks.GetProjectTasksPaged
{
    public class GetProjectTasksPagedRequest : IRequest<ResponseBase<GetProjectTasksPagedResponse>>
    {
        public Guid ProjectId { get; set; }
        public Guid? Status { get; set; }
        public Guid? AssigneeId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}