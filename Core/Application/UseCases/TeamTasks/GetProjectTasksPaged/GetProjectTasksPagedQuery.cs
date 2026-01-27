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

namespace Application.UseCases.TeamTasks.GetProjectTasksPaged
{
    public class GetProjectTasksPagedQuery(
    ITasksRepository tasksRepository,
    IMapperAdapter mapper
) : IRequestHandler<GetProjectTasksPagedRequest, ResponseBase<GetProjectTasksPagedResponse>>
    {
        private readonly ITasksRepository _tasksRepository = tasksRepository;
        private readonly IMapperAdapter _mapperAdapter = mapper;

        public async Task<ResponseBase<GetProjectTasksPagedResponse>> Handle(GetProjectTasksPagedRequest request, CancellationToken cancellationToken)
        {
            var (items, total) = await _tasksRepository.GetProjectTasksPagedAsync(
                request.ProjectId,
                request.Status,
                request.AssigneeId,
                request.Page,
                request.PageSize,
                cancellationToken);

            var response = new ResponseBase<GetProjectTasksPagedResponse>
            {
                Data = new GetProjectTasksPagedResponse
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalCount = total,
                    Tasks = _mapperAdapter.Map<ProjectTaskPagedRow, TaskDto>(items)
                }
            };

            return response;
        }
    }
}