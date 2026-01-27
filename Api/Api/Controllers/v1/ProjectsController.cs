using Application.UseCases.TeamTasks.GetProjects;
using Application.UseCases.TeamTasks.GetProjectTasksPaged;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProjectsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetProjects(CancellationToken ct)
            => Ok(await _mediator.Send(new GetProjectsRequest(), ct));

        [HttpGet("{id:guid}/tasks")]
        public async Task<IActionResult> GetProjectTasks(GetProjectTasksPagedRequest req, CancellationToken ct)
        { 
            return Ok(await _mediator.Send(req, ct));
        }
    }
}