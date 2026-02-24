using Application.Base;
using Application.Dtos;
using Domain.Contracts.Adapter.Mapper;
using Domain.Contracts.Persistence;
using MediatR;
using System.Net;
using static Application.Enums.Enums;

namespace Application.UseCases.TeamTasks.UpdateTaskStatus;

public sealed class UpdateTaskStatusCommand(
    ITasksRepository tasksRepository,
    IMapperAdapter mapperAdapter
) : IRequestHandler<UpdateTaskStatusRequest, ResponseBase<UpdateTaskStatusResponse>>
{
    private readonly ITasksRepository _tasksRepository = tasksRepository;
    private readonly IMapperAdapter _mapperAdapter = mapperAdapter;

    public async Task<ResponseBase<UpdateTaskStatusResponse>> Handle(UpdateTaskStatusRequest request, CancellationToken cancellationToken)
    {
        var response = new ResponseBase<UpdateTaskStatusResponse>();

        if (!Enum.TryParse<EnumTaskStatus>(request.Status, true, out var targetStatus))
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.Message = "Status inválido.";
            response.Data = default!;
            return response;
        }

        var task = await _tasksRepository.FindAsync(request.TaskId);
        if (task is null)
        {
            response.StatusCode = HttpStatusCode.NotFound;
            response.Message = "Task no encontrada.";
            response.Data = default!;
            return response;
        }

        var pendingId = Application.Constants.Constants.TaskStatusDetailIds[EnumTaskStatus.Pending];
        var doneId = Application.Constants.Constants.TaskStatusDetailIds[EnumTaskStatus.Done];
        var targetStatusId = Application.Constants.Constants.TaskStatusDetailIds[targetStatus];

        // Regla de negocio: Pending -> Done NO permitido
        if (task.StatusId == pendingId && targetStatusId == doneId)
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.Message = "Transición inválida: Pending -> Done no está permitido.";
            response.Data = default!;
            return response;
        }

        task.StatusId = targetStatusId;
        task.UpdatedAtUtc = DateTime.UtcNow;

        try
        {
            _tasksRepository.Edit(task);
        }
        catch (Exception)
        {

            throw;
        }


        var dto = _mapperAdapter.Map<Domain.Entities.TaskEntity, TaskStatusUpdatedDto>(task);

        dto.Status = targetStatus.ToString();

        response.Message = "Estado actualizado.";
        response.Data = new UpdateTaskStatusResponse { Task = dto };

        return response;
    }
}