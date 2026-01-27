using Application.Base;
using Application.Dtos.TeamTasks;
using Domain.Contracts.Persistence;
using Domain.QueryModels;
using MediatR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.TeamTasks.CreateTask
{
    public class CreateTaskCommand(ITasksRepository tasksRepository)
    : IRequestHandler<CreateTaskRequest, ResponseBase<CreateTaskResponse>>
    {
        private readonly ITasksRepository _tasksRepository = tasksRepository;

        public async Task<ResponseBase<CreateTaskResponse>> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var input = new CreateTaskSpParams
                {
                    ProjectId = request.ProjectId,
                    Title = request.Title.Trim(),
                    Description = request.Description,
                    AssigneeId = request.AssigneeId,
                    IdTaskStatus = request.IdTaskStatus,
                    IdTaskPriority = request.IdTaskPriority,
                    EstimatedComplexity = request.EstimatedComplexity,
                    DueDate = request.DueDate.Date,
                    CompletionDate = request.CompletionDate?.Date
                };

                var created = await _tasksRepository.CreateTaskAsync(input, cancellationToken);

                return new ResponseBase<CreateTaskResponse>
                {
                    Data = new CreateTaskResponse
                    {
                        Task = new TaskDto
                        {
                            TaskId = created.TaskId,
                            ProjectId = created.ProjectId,
                            ProjectName = created.ProjectName,
                            Title = created.Title,
                            Description = created.Description,
                            AssigneeId = created.AssigneeId,
                            AssigneeName = created.AssigneeName,
                            IdTaskStatus = created.IdTaskStatus,
                            Status = created.EstadoTarea,
                            IdTaskPriority = created.IdTaskPriority,
                            Priority = created.Prioridad,
                            EstimatedComplexity = created.EstimatedComplexity,
                            DueDate = created.DueDate,
                            CompletionDate = created.CompletionDate,
                            CreatedAts = created.CreatedAts
                        }
                    }
                };
            }
            catch (SqlException ex) when (ex.Number == 51000)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}