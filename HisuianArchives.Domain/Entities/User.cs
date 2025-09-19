namespace HisuianArchives.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } 

    public string? Bio { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set; }

    public Guid? ProfileImageId { get; private set; }

    public virtual ICollection<Role> Roles { get; private set; } = new List<Role>();


    private User() { }


    public User(string name, string email, string passwordHash, string? bio)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Bio = bio;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}