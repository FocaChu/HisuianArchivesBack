using FluentValidation;
using HisuianArchives.Domain.Entities;

namespace HisuianArchives.Application.Features.Images.Commands.UploadImage;

/// <summary>
/// Validator for the UploadImageCommand to ensure all required fields are provided and valid.
/// </summary>
public class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
{
    public UploadImageCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required.")
            .Must(file => file != null && file.Length > 0)
            .WithMessage("File cannot be empty.");

        RuleFor(x => x.ImageType)
            .NotEmpty()
            .WithMessage("Image type is required.")
            .Must(BeValidImageType)
            .WithMessage("Invalid ImageType specified.");
    }

    private static bool BeValidImageType(string imageType)
    {
        return Enum.TryParse<ImageType>(imageType, true, out _);
    }
}