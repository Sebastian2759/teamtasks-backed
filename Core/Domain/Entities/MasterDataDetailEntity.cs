namespace Domain.Entities;

public sealed class MasterDataDetailEntity : Base.EntityBase
{
    public Guid MasterDataId { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}
