using Application.Constants;
using Application.Dtos;
using Application.UseCases.TeamTasks.UpdateTaskStatus;
using Domain.Contracts.Adapter.Mapper;
using Domain.Contracts.Persistence;
using Domain.Entities;
using Moq;
using System.Net;
using static Application.Enums.Enums;
using FluentAssertions;


namespace Test.Core.Application.UsesCases.TeamTasks;

public class UpdateTaskStatusCommandTests
{
    private readonly Mock<ITasksRepository> _tasksRepo = new();
    private readonly Mock<IMapperAdapter> _mapper = new();

    private UpdateTaskStatusCommand CreateSut()
        => new(_tasksRepo.Object, _mapper.Object);

    [Fact]
    public async Task Handle_StatusInvalido_ReturnsBadRequest_AndDoesNotEdit()
    {
        // Arrange
        var sut = CreateSut();
        var req = new UpdateTaskStatusRequest(Guid.NewGuid(), "INVALID_STATUS");

        // Act
        var res = await sut.Handle(req, CancellationToken.None);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        res.Message.Should().NotBeNullOrWhiteSpace();

        _tasksRepo.Verify(r => r.Edit(It.IsAny<TaskEntity>()), Times.Never);
        _tasksRepo.Verify(r => r.FindAsync(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public async Task Handle_TaskNotFound_ReturnsNotFound()
    {
        // Arrange
        var sut = CreateSut();
        var req = new UpdateTaskStatusRequest(Guid.NewGuid(), "InProgress");

        _tasksRepo
            .Setup(r => r.FindAsync(It.IsAny<object>()))
            .ReturnsAsync((TaskEntity?)null);

        // Act
        var res = await sut.Handle(req, CancellationToken.None);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
        res.Message.Should().NotBeNullOrWhiteSpace();

        _tasksRepo.Verify(r => r.Edit(It.IsAny<TaskEntity>()), Times.Never);
    }

    [Fact]
    public async Task Handle_PendingToDone_NotAllowed_ReturnsBadRequest_AndDoesNotEdit()
    {
        // Arrange
        var sut = CreateSut();

        var task = new TaskEntity
        {
            Id = Guid.NewGuid(),
            Title = "T",
            AssignedUserId = Guid.NewGuid(),
            StatusId = Constants.TaskStatusDetailIds[EnumTaskStatus.Pending],
            CreatedAtUtc = DateTime.UtcNow.AddDays(-1),
            UpdatedAtUtc = DateTime.UtcNow.AddDays(-1),
            IsActive = true
        };

        _tasksRepo
            .Setup(r => r.FindAsync(It.IsAny<object>()))
            .ReturnsAsync(task);

        var req = new UpdateTaskStatusRequest(task.Id, "Done");

        // Act
        var res = await sut.Handle(req, CancellationToken.None);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        res.Message.Should().Contain("Pending").And.Contain("Done");

        _tasksRepo.Verify(r => r.Edit(It.IsAny<TaskEntity>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ValidTransition_UpdatesStatus_CallsEdit_ReturnsOkWithDto()
    {
        // Arrange
        var sut = CreateSut();

        var originalUpdated = DateTime.UtcNow.AddHours(-2);

        var task = new TaskEntity
        {
            Id = Guid.NewGuid(),
            Title = "T",
            AssignedUserId = Guid.NewGuid(),
            StatusId = Constants.TaskStatusDetailIds[EnumTaskStatus.Pending],
            CreatedAtUtc = DateTime.UtcNow.AddDays(-1),
            UpdatedAtUtc = originalUpdated,
            IsActive = true
        };

        _tasksRepo
            .Setup(r => r.FindAsync(It.IsAny<object>()))
            .ReturnsAsync(task);

        // Mapeo DTO (ajusta el DTO real si tu nombre/props varían)
        _mapper
            .Setup(m => m.Map<TaskEntity, TaskStatusUpdatedDto>(It.IsAny<TaskEntity>()))
            .Returns(new TaskStatusUpdatedDto
            {
                Id = task.Id,
                StatusId = task.StatusId
            });

        _tasksRepo.Setup(r => r.Edit(It.IsAny<TaskEntity>()));

        var req = new UpdateTaskStatusRequest(task.Id, "InProgress");

        // Act
        var res = await sut.Handle(req, CancellationToken.None);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        res.Data.Should().NotBeNull();
        res.Data.Task.Should().NotBeNull();

        // Validar que la entidad cambió
        task.StatusId.Should().Be(Constants.TaskStatusDetailIds[EnumTaskStatus.InProgress]);
        task.UpdatedAtUtc.Should().BeAfter(originalUpdated);

        // Validar que guardó
        _tasksRepo.Verify(r => r.Edit(It.IsAny<TaskEntity>()), Times.Once);
    }
}
