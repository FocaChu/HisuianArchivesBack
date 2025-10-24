using Microsoft.AspNetCore.Http;

namespace HisuianArchives.Application.DTOs.Image;

public class ImageUploadRequestDto
{
    public IFormFile File { get; set; } = null!;

    public string ImageType { get; set; } = string.Empty; 
}