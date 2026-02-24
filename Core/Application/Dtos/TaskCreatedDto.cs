namespace Application.Dtos;

public sealed class TaskCreatedDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public Guid AssignedUserId { get; set; }
    public Guid StatusId { get; set; }
}
