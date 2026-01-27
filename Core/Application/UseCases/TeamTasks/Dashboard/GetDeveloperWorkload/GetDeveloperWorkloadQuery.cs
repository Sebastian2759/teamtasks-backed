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

namespace Application.UseCases.TeamTasks.Dashboard.GetDeveloperWorkload
{
    public class GetDeveloperWorkloadQuery(
     IDashboardRepository dashboardRepository,
     IMapperAdapter mapper
 ) : IRequestHandler<GetDeveloperWorkloadRequest, ResponseBase<GetDeveloperWorkloadResponse>>
    {
        private readonly IDashboardRepository _dashboardRepository = dashboardRepository;
        private readonly IMapperAdapter _mapperAdapter = mapper;

        public async Task<ResponseBase<GetDeveloperWorkloadResponse>> Handle(GetDeveloperWorkloadRequest request, CancellationToken cancellationToken)
        {
            var rows = await _dashboardRepository.GetDeveloperWorkloadAsync(cancellationToken);

            return new ResponseBase<GetDeveloperWorkloadResponse>
            {
                Data = new GetDeveloperWorkloadResponse
                {
                    Developers = _mapperAdapter.Map<DeveloperWorkloadRow, DeveloperWorkloadDto>(rows)
                }
            };
        }
    }
}