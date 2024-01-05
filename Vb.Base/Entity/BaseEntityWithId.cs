namespace Vb.Base.Entity;

public abstract class BaseEntityWithId : BaseEntity
{
    public int Id { get; set; }
}

public abstract class BaseEntity //Main class in every entity
{
    public int InsertUserId { get; set; }
    public DateTime InsertDate { get; set; }
    public int? UpdateUserId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool? IsActive { get; set; }
}