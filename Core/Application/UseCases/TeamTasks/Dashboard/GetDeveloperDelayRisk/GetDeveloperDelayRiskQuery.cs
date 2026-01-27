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

namespace Application.UseCases.TeamTasks.Dashboard.GetDeveloperDelayRisk
{
    public class GetDeveloperDelayRiskQuery(
    IDashboardRepository dashboardRepository,
    IMapperAdapter mapper
) : IRequestHandler<GetDeveloperDelayRiskRequest, ResponseBase<GetDeveloperDelayRiskResponse>>
    {
        private readonly IDashboardRepository _dashboardRepository = dashboardRepository;
        private readonly IMapperAdapter _mapperAdapter = mapper;

        public async Task<ResponseBase<GetDeveloperDelayRiskResponse>> Handle(GetDeveloperDelayRiskRequest request, CancellationToken cancellationToken)
        {
            var rows = await _dashboardRepository.GetDeveloperDelayRiskAsync(cancellationToken);

            return new ResponseBase<GetDeveloperDelayRiskResponse>
            {
                Data = new GetDeveloperDelayRiskResponse
                {
                    Developers = _mapperAdapter.Map<DeveloperDelayRiskRow, DeveloperDelayRiskDto>(rows)
                }
            };
        }
    }
}