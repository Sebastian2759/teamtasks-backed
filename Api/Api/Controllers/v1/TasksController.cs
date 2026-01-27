using Application.UseCases.TeamTasks.CreateTask;
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

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(request, ct));
}