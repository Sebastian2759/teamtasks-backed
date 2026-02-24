using Application.Base;
using MediatR;

namespace Application.UseCases.User.CreateUser;

public class CreateUserRequest : IRequest<ResponseBase<CreateUserResponse>>
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
}
