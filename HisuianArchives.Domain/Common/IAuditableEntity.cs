namespace HisuianArchives.Domain.Common
{
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; }

        DateTime UpdatedAt { get; set; } 
    }
}
