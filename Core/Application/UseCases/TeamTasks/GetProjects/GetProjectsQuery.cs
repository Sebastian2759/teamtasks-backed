using Application.Base;
using Application.Dtos.TeamTasks;
using Domain.Contracts.Adapter.Mapper;
using Domain.Contracts.Persistence;
using Domain.QueryModels;
using MediatR;

namespace Application.UseCases.TeamTasks.GetProjects;

public class GetProjectsQuery(
    IDashboardRepository dashboardRepository,
    IMapperAdapter mapper
) : IRequestHandler<GetProjectsRequest, ResponseBase<GetProjectsResponse>>
{
    private readonly IDashboardRepository _dashboardRepository = dashboardRepository;
    private readonly IMapperAdapter _mapperAdapter = mapper;

    public async Task<ResponseBase<GetProjectsResponse>> Handle(GetProjectsRequest request, CancellationToken cancellationToken)
    {
        List<ProjectHealthRow> rows = await _dashboardRepository.GetProjectHealthAsync(cancellationToken);

        ResponseBase<GetProjectsResponse> response = new ()
        {
            Data = new GetProjectsResponse
            {
                Projects = _mapperAdapter.Map<ProjectHealthRow, ProjectSummaryDto>(rows)
            }
        };

        return response;
    }
}
