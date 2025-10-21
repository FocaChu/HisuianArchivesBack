using HisuianArchives.Domain.Repositories;
using HisuianArchives.Infrastructure.Persistence.Data;

namespace HisuianArchives.Infrastructure.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly HisuianArchivesDbContext _context;

    public ImageRepository(HisuianArchivesDbContext context)
    {
        _context = context;
    }

    public async Task<Image?> GetByIdAsync(Guid id)
    {
        return await _context.Images.FindAsync(id);
    }

    public async Task AddAsync(Image image)
    {
        await _context.Images.AddAsync(image);
        await _context.SaveChangesAsync(); 
    }
}