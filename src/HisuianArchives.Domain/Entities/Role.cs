public class Role : BaseEntity<Guid>
{
    public string Name { get; private set; }
    public virtual ICollection<User> Users { get; private set; } = new List<User>();

    private Role() { }

    public Role(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Role name cannot be empty.", nameof(name));

        Name = name;
    }

    public Role(Guid id, string name) : this(name)
    {
        Id = id;
    }
}