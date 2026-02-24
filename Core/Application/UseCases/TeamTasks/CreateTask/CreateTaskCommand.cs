using Application.Base;
using Application.Dtos;
using Domain.Contracts.Adapter.Mapper;
using Domain.Contracts.Persistence;
using Domain.Entities;
using MediatR;
using System.Net;
using static Application.Enums.Enums;

namespace Application.UseCases.TeamTasks.CreateTask;

public sealed class CreateTaskCommand(
    ITasksRepository tasksRepository,
    IUsersRepository usersRepository,
    IMapperAdapter mapperAdapter
) : IRequestHandler<CreateTaskRequest, ResponseBase<CreateTaskResponse>>
{
    private readonly ITasksRepository _tasksRepository = tasksRepository;
    private readonly IUsersRepository _usersRepository = usersRepository;
    private readonly IMapperAdapter _mapperAdapter = mapperAdapter;

    public async Task<ResponseBase<CreateTaskResponse>> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var response = new ResponseBase<CreateTaskResponse>();

        // negocio: usuario debe existir
        var userExists = await _usersRepository.ExistsAsync(
            u => u.Id == request.AssignedUserId && u.IsActive,
            cancellationToken);

        if (!userExists)
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.Message = "AssignedUserId no existe o está inactivo.";
            response.Data = default!;
            return response;
        }

        var now = DateTime.UtcNow;
        var pendingStatusId = Application.Constants.Constants.TaskStatusDetailIds[EnumTaskStatus.Pending];

        var entity = new TaskEntity
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Description = request.Description?.Trim(),
            AssignedUserId = request.AssignedUserId,
            StatusId = pendingStatusId,
            PriorityId = request.PriorityId,
            AdditionalInfo = string.IsNullOrWhiteSpace(request.AdditionalInfo) ? null : request.AdditionalInfo,
            IsActive = true,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        try
        {
            _tasksRepository.Add(entity);
        }
        catch (Exception)
        {

            throw;
        }


        // ✅ Mapeo con adapter
        var dto = _mapperAdapter.Map<TaskEntity, TaskCreatedDto>(entity);

        response.StatusCode = HttpStatusCode.Created;
        response.Message = "Tarea creada.";
        response.Data = new CreateTaskResponse { Task = dto };

        return response;
    }
}