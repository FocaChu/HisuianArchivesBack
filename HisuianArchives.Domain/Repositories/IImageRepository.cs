namespace HisuianArchives.Domain.Repositories;

public interface IImageRepository
{
    Task<Image?> GetByIdAsync(Guid id);

    Task AddAsync(Image image);
}