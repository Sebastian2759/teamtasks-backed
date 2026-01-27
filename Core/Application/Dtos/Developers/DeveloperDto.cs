namespace Application.Dtos.Developers;

public class DeveloperDto
{
    public Guid DeveloperId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
