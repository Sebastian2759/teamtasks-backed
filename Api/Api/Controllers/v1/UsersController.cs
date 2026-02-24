using Application.UseCases.User.CreateUser;
using Application.UseCases.User.GetUsers;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken ct)
            => Ok(await _mediator.Send(request, ct));

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] GetUsersRequest request, CancellationToken ct)
            => Ok(await _mediator.Send(request, ct));
    }
}
