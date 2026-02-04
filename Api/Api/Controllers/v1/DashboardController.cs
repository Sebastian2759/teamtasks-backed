using Application.UseCases.TeamTasks.Dashboard.GetDeveloperDelayRisk;
using Application.UseCases.TeamTasks.Dashboard.GetDeveloperWorkload;
using Application.UseCases.TeamTasks.Dashboard.GetProjectHealth;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DashboardController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("developer-workload")]
        public async Task<IActionResult> DeveloperWorkload(CancellationToken ct)
            =>  Ok(await _mediator.Send(new GetDeveloperWorkloadRequest(), ct));

        [HttpGet("project-health")]
        public async Task<IActionResult> ProjectHealth(CancellationToken ct)
            => Ok(await _mediator.Send(new GetProjectHealthRequest(), ct));

        [HttpGet("developer-delay-risk")]
        public async Task<IActionResult> DeveloperDelayRisk(CancellationToken ct)
            => Ok(await _mediator.Send(new GetDeveloperDelayRiskRequest(), ct));
    }
}