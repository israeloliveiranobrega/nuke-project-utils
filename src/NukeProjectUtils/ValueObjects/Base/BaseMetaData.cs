namespace NukeProjectUtils.ValueObjects.Base;

public record BaseMetaData
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }
    public DateTime CreateDate { get; set; }

    public Guid? LastUpdateBy { get; set; }
    public DateTime? LastUpdateDate { get; set; }

    public Guid? SuspendedBy { get; set; }
    public DateTime? SuspendedDate { get; set; }

    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }

    public Guid? ActivedBy { get; set; }
    public DateTime? ActivedDate { get; set; }

    public uint Version { get; set; }
}
