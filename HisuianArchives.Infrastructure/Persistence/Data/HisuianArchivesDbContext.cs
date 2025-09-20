using HisuianArchives.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HisuianArchives.Infrastructure.Persistence.Data;

public class HisuianArchivesDbContext : DbContext
{
    public HisuianArchivesDbContext(DbContextOptions<HisuianArchivesDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.HasIndex(u => u.Email).IsUnique();

            entity.HasMany(u => u.Roles)
                  .WithMany(r => r.Users)
                  .UsingEntity(j => j.ToTable("UserRoles"));
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.HasIndex(r => r.Name).IsUnique();
        });


        var customerRoleId = Guid.NewGuid();
        var proRoleId = Guid.NewGuid();
        var adminRoleId = Guid.NewGuid();

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = customerRoleId, Name = "Customer" },
            new Role { Id = proRoleId, Name = "Pro" },
            new Role { Id = adminRoleId, Name = "Admin" }
        );
    }
}