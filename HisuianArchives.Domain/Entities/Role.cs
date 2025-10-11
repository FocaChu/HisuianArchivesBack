namespace HisuianArchives.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}