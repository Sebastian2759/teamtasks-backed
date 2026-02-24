namespace Application.Dtos;

public sealed class UserCreatedDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
}
