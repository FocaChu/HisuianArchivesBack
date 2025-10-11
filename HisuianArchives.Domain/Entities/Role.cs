namespace HisuianArchives.Domain.Entities;

public class Role
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public virtual ICollection<User> Users { get; private set; } = new List<User>();

    private Role() { }

    public Role(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}