using Application.UseCases.Developers.GetActiveDevelopers;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Context;

namespace Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DevelopersController : ControllerBase
{

    private readonly IMediator _mediator;

    public DevelopersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetActiveDevelopers()
    {
        var result = await _mediator.Send(new GetActiveDevelopersRequest());
        return Ok(result.Data);
    }
}