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


        // Use fixed GUIDs for seeded roles to ensure consistency across environments
        var customerRoleId = new Guid("11111111-1111-1111-1111-111111111111");
        var proRoleId = new Guid("22222222-2222-2222-2222-222222222222");
        var adminRoleId = new Guid("33333333-3333-3333-3333-333333333333");

        // Create roles and set IDs using reflection for seeding
        var customerRole = new Role("Customer");
        var proRole = new Role("Pro");
        var adminRole = new Role("Admin");

        // Set private Id property using reflection
        typeof(Role).GetProperty("Id")!.SetValue(customerRole, customerRoleId);
        typeof(Role).GetProperty("Id")!.SetValue(proRole, proRoleId);
        typeof(Role).GetProperty("Id")!.SetValue(adminRole, adminRoleId);

        modelBuilder.Entity<Role>().HasData(customerRole, proRole, adminRole);
    }
}