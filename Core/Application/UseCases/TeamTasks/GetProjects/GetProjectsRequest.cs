using Application.Base;
using MediatR;

namespace Application.UseCases.TeamTasks.GetProjects;

public class GetProjectsRequest : IRequest<ResponseBase<GetProjectsResponse>>
{
}
