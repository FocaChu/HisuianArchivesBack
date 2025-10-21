using MediatR;
using HisuianArchives.Application.DTOs.Image;
using Microsoft.AspNetCore.Http;

namespace HisuianArchives.Application.Features.Images.Commands.UploadImage;

/// <summary>
/// Command for uploading an image file.
/// </summary>
public class UploadImageCommand : IRequest<ImageUploadResponseDto>
{
    public IFormFile File { get; set; } = null!;
    public string ImageType { get; set; } = null!;
    public Guid UserId { get; set; }
}
