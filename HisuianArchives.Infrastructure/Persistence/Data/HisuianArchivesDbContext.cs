using HisuianArchives.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HisuianArchives.Infrastructure.Persistence.Data;

public class HisuianArchivesDbContext : DbContext
{
    public HisuianArchivesDbContext(DbContextOptions<HisuianArchivesDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }
}