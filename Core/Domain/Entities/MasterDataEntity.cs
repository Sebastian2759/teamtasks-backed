namespace Domain.Entities;

public sealed class MasterDataEntity : AuditableEntity
{
    public string Name { get; set; } = null!;
}
