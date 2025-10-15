using HisuianArchives.Domain.Entities;
using HisuianArchives.Domain.Repositories;
using HisuianArchives.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace HisuianArchives.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HisuianArchivesDbContext _context;

    public UserRepository(HisuianArchivesDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync(); 
    }

    public async Task<User?> GetUserByEmailWithRolesAsync(string email)
    {
        return await _context.Users
                             .Include(u => u.Roles) 
                             .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}