using MediatR;
using HisuianArchives.Application.DTOs.Image;
using HisuianArchives.Application.Interfaces;
using HisuianArchives.Domain.Entities;
using HisuianArchives.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace HisuianArchives.Application.Features.Images.Commands.UploadImage;

/// <summary>
/// Handler for the UploadImageCommand that uploads an image file and saves it to the database.
/// </summary>
public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, ImageUploadResponseDto>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IImageRepository _imageRepository;
    private readonly ILogger<UploadImageCommandHandler> _logger;

    public UploadImageCommandHandler(
        IFileStorageService fileStorageService,
        IImageRepository imageRepository,
        ILogger<UploadImageCommandHandler> logger)
    {
        _fileStorageService = fileStorageService;
        _imageRepository = imageRepository;
        _logger = logger;
    }

    public async Task<ImageUploadResponseDto> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Uploading image for user {UserId} with type {ImageType}", request.UserId, request.ImageType);

        // Validate file
        if (request.File == null || request.File.Length == 0)
        {
            _logger.LogWarning("Upload failed: no file provided for user {UserId}", request.UserId);
            throw new BusinessException("No file was uploaded.");
        }

        // Validate image type
        if (!Enum.TryParse<ImageType>(request.ImageType, true, out var imageType))
        {
            _logger.LogWarning("Upload failed: invalid image type {ImageType} for user {UserId}", request.ImageType, request.UserId);
            throw new BusinessException("Invalid ImageType specified.");
        }

        // Upload file to storage
        await using var stream = request.File.OpenReadStream();
        var uploadResult = await _fileStorageService.UploadFileAsync(stream, request.File.FileName, request.File.ContentType);

        // Create image entity
        var image = new Image(
            ownerId: request.UserId,
            url: uploadResult.Url,
            fileName: uploadResult.FileName,
            sizeInBytes: request.File.Length,
            type: imageType
        );

        // Save to database
        await _imageRepository.AddAsync(image);

        _logger.LogInformation("Image uploaded successfully for user {UserId} with ID {ImageId}", request.UserId, image.Id);

        return new ImageUploadResponseDto
        {
            ImageId = image.Id,
            Url = image.Url
        };
    }
}

