using Domain.Base;

namespace Domain.Entities;

public class ReferencialDataEntity : EntityBase
{
    public Guid Id { get; set; }
    public string Description { get; set; } = default!;
    public string? AuxiliarData { get; set; }
    public bool Valid { get; set; }
    public DateTime RegisterDate { get; set; }
    public string? RegisterUser { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string? UpdateUser { get; set; }
}