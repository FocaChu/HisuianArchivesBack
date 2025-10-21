namespace HisuianArchives.Application.DTOs.Image;

public class ImageUploadResponseDto
{
    public Guid ImageId { get; set; }

    public string Url { get; set; } = string.Empty;
}