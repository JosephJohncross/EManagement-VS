namespace EManagementVSA.Shared.Entities;

public class BaseEntity
{
    public BaseEntity()
    {
        CreatedAt = DateTime.UtcNow;
        ModifiedAt = CreatedAt;
    }

    public DateTime CreatedAt { get; }
    public string? CreatedBy { get; set; }
    public DateTime ModifiedAt { get; }
    public string? ModifiedBy { get; set; }
}