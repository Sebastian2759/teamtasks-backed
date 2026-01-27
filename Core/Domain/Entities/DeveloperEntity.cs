namespace Domain.Entities;

public class DeveloperEntity
{
    public Guid DeveloperId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime CreatedAts { get; set; }
    public string FullName => $"{FirstName} {LastName}".Trim();
}
