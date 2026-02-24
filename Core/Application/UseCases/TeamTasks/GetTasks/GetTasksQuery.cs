using Application.Base;
using Domain.Contracts.Persistence;
using MediatR;

namespace Application.UseCases.TeamTasks.GetTasks;

public class GetTasksQuery(ITasksRepository tasksRepository)
    : IRequestHandler<GetTasksRequest, ResponseBase<GetTasksResponse>>
{
    private readonly ITasksRepository _tasksRepository = tasksRepository;

    public async Task<ResponseBase<GetTasksResponse>> Handle(GetTasksRequest request, CancellationToken cancellationToken)
    {
        var response = new ResponseBase<GetTasksResponse>();

        var page = await _tasksRepository.GetTasksPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.AssignedUserId,
            request.Status,
            request.PriorityId,
            request.Tag,
            request.Search,
            cancellationToken);

        response.Data = new GetTasksResponse { Page = page };
        return response;
    }
}