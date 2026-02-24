using Application.UseCases.Values.GetValues;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Application.Enums.Enums;

namespace Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public sealed class ValuesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("task-statuses")]
        public async Task<IActionResult> GetTaskStatuses(CancellationToken ct)
        {
            var res = await _mediator.Send(new GetValuesRequest { Type = ValuesType.TaskStatuses }, ct);
            return StatusCode((int)res.StatusCode, res);
        }

        [HttpGet("task-priorities")]
        public async Task<IActionResult> GetTaskPriorities(CancellationToken ct)
        {
            var res = await _mediator.Send(new GetValuesRequest { Type = ValuesType.TaskPriorities }, ct);
            return StatusCode((int)res.StatusCode, res);
        }
    }
}