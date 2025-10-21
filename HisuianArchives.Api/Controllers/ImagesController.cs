using HisuianArchives.Api.Extensions;
using HisuianArchives.Application.DTOs.Image;
using HisuianArchives.Domain.Entities;
using HisuianArchives.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HisuianArchives.Api.Controllers;

[ApiController]
[Route("api/images")]
[Authorize]
public class ImagesController : ControllerBase
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IImageRepository _imageRepository; 

    public ImagesController(IFileStorageService fileStorageService, IImageRepository imageRepository)
    {
        _fileStorageService = fileStorageService;
        _imageRepository = imageRepository;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage([FromForm] ImageUploadRequestDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
        {
            return BadRequest("No file was uploaded.");
        }

        if (!Enum.TryParse<ImageType>(dto.ImageType, true, out var imageType))
        {
            return BadRequest("Invalid ImageType specified.");
        }

        await using var stream = dto.File.OpenReadStream();
        var uploadResult = await _fileStorageService.UploadFileAsync(stream, dto.File.FileName, dto.File.ContentType);

        var userId = User.GetCurrentUserId()!;
        var image = new Image(
            ownerId: userId,
            url: uploadResult.Url,
            fileName: uploadResult.FileName,
            sizeInBytes: dto.File.Length,
            type: imageType
        );

        await _imageRepository.AddAsync(image);

        var responseDto = new ImageUploadResponseDto
        {
            ImageId = image.Id,
            Url = image.Url
        };
        return Ok(responseDto);
    }
}