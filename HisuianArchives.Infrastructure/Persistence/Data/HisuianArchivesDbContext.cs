using Microsoft.EntityFrameworkCore;
using System.Reflection; 

namespace HisuianArchives.Infrastructure.Persistence.Data;

public class HisuianArchivesDbContext : DbContext
{
    public HisuianArchivesDbContext(DbContextOptions<HisuianArchivesDbContext> options) : base(options) { }

    public DbSet<Role> Roles { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Image> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}