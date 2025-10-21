namespace HisuianArchives.Domain.Entities;

public enum ImageType
{
    UserProfile,
    PokemonProfile,
    CustomType,
    CustomTypeTCG,
    CardTCG,
    Other
}

public class Image : BaseEntity<Guid>
{
    public Guid OwnerId { get; private set; }

    public virtual User Owner { get; private set; } = null!;

    public string Url { get; private set; }
    public string FileName { get; private set; }
    public long SizeInBytes { get; private set; }
    public ImageType Type { get; private set; }

    private Image() { }

    public Image(Guid ownerId, string url, string fileName, long sizeInBytes, ImageType type)
    {
        if (ownerId == Guid.Empty)
            throw new ArgumentException("OwnerId cannot be empty.", nameof(ownerId));

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be empty.", nameof(url));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("FileName cannot be empty.", nameof(fileName));

        if (sizeInBytes <= 0)
            throw new ArgumentException("SizeInBytes must be a positive number.", nameof(sizeInBytes));

        Id = Guid.NewGuid();
        OwnerId = ownerId;
        Url = url;
        FileName = fileName;
        SizeInBytes = sizeInBytes;
        Type = type;
    }
}