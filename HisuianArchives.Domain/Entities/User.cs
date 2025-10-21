namespace HisuianArchives.Domain.Entities;

public class User : BaseAuditableEntity<Guid>
{
    public string Name { get; private set; } 

    public string? Bio { get; private set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set; }

    public Guid? ProfileImageId { get; private set; }

    public Image? ProfileImage { get; private set; }

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

        this.AddDomainEvent(new UserCreatedEvent(this));
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

    public void SetProfileImage(Guid? imageId)
    {
         ProfileImageId = imageId;
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