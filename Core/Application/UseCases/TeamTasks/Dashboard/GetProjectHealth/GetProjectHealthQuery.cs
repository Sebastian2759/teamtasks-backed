using Application.Base;
using Application.Dtos.TeamTasks;
using Domain.Contracts.Adapter.Mapper;
using Domain.Contracts.Persistence;
using Domain.QueryModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.TeamTasks.Dashboard.GetProjectHealth
{
    public class GetProjectHealthQuery(
     IDashboardRepository dashboardRepository,
     IMapperAdapter mapper
 ) : IRequestHandler<GetProjectHealthRequest, ResponseBase<GetProjectHealthResponse>>
    {
        private readonly IDashboardRepository _dashboardRepository = dashboardRepository;
        private readonly IMapperAdapter _mapperAdapter = mapper;

        public async Task<ResponseBase<GetProjectHealthResponse>> Handle(GetProjectHealthRequest request, CancellationToken cancellationToken)
        {
            var rows = await _dashboardRepository.GetProjectHealthAsync(cancellationToken);

            return new ResponseBase<GetProjectHealthResponse>
            {
                Data = new GetProjectHealthResponse
                {
                    Projects = _mapperAdapter.Map<ProjectHealthRow, ProjectHealthDto>(rows)
                }
            };
        }
    }
}