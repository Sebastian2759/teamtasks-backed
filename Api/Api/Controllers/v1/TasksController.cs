using Application.UseCases.TeamTasks.CreateTask;
using Application.UseCases.TeamTasks.GetTasks;
using Application.UseCases.TeamTasks.UpdateTaskStatus;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TasksController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;


    // POST /api/v1/tasks
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(request, ct));

    // GET /api/v1/tasks?PageNumber=1&PageSize=20&AssignedUserId=...&Status=Pending&Priority=High
    // (Listas paginadas y filtrables: perfecto para SP)
    [HttpGet]
    public async Task<IActionResult> GetTasks([FromQuery] GetTasksRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(request, ct));

    // PUT /api/v1/tasks/{id}/status  body: { "status": "InProgress" }
    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] UpdateTaskStatusRequest body, CancellationToken ct)
    {
        try
        {
            var res = await _mediator.Send(new UpdateTaskStatusRequest(id, body.Status), ct);
            return StatusCode((int)res.StatusCode, res);
        }
        catch (Exception)
        {

            throw;
        }

    }
}