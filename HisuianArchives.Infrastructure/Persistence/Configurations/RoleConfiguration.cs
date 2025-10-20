using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HisuianArchives.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .HasMaxLength(35)
            .IsRequired();

        builder.HasIndex(r => r.Name).IsUnique();

        builder.HasData(
            new Role(new Guid("11111111-1111-1111-1111-111111111111"), "Customer"),
            new Role(new Guid("22222222-2222-2222-2222-222222222222"), "Pro"),
            new Role(new Guid("33333333-3333-3333-3333-333333333333"), "Admin")
        );
    }
}