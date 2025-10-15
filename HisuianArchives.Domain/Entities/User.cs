using HisuianArchives.Domain.Common;

namespace HisuianArchives.Domain.Entities;

public class User : IAuditableEntity
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } 

    public string? Bio { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set; }

    public virtual ICollection<Role> Roles { get; private set; } = new List<Role>();

    private User() { }

    public User(string name, string email, string passwordHash, string? bio)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));
            
        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format", nameof(email));
        
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));

        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Bio = bio;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(string name, string? bio)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
            
        Name = name;
        Bio = bio;
    }
    
    public void UpdateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));
            
        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format", nameof(email));
            
        Email = email;
    }
    
    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Password hash cannot be empty", nameof(newPasswordHash));
            
        PasswordHash = newPasswordHash;
    }
    
    public void AddRole(Role role)
    {
        if (role == null)
            throw new ArgumentNullException(nameof(role));
            
        if (!Roles.Any(r => r.Id == role.Id))
        {
            Roles.Add(role);
        }
    }
    
    public void RemoveRole(Role role)
    {
        if (role == null)
            throw new ArgumentNullException(nameof(role));
            
        Roles.Remove(role);
    }
    
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}