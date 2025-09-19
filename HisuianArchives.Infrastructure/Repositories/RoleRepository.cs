using HisuianArchives.Domain.Entities;
using HisuianArchives.Domain.Repositories;
using HisuianArchives.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace HisuianArchives.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly HisuianArchivesDbContext _context;
    public RoleRepository(HisuianArchivesDbContext context) { _context = context; }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name); ;
    }
}
