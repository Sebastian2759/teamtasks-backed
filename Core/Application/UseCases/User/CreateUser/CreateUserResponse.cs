using Application.Dtos;

namespace Application.UseCases.User.CreateUser;

public class CreateUserResponse
{
    public UserCreatedDto User { get; set; } = default!;
}
