using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HisuianArchives.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(60)
            .IsRequired();

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasMany(u => u.Roles)
               .WithMany(r => r.Users)
               .UsingEntity(j => j.ToTable("UserRoles"));
    }
}