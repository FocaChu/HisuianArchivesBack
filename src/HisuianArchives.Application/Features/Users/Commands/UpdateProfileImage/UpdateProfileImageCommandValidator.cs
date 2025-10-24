using FluentValidation;

namespace HisuianArchives.Application.Features.Users.Commands.UpdateProfileImage;

/// <summary>
/// Validator for the UpdateProfileImageCommand to ensure all required fields are provided and valid.
/// </summary>
public class UpdateProfileImageCommandValidator : AbstractValidator<UpdateProfileImageCommand>
{
    public UpdateProfileImageCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(x => x.ImageId)
            .NotEmpty()
            .WithMessage("Image ID is required.");
    }
}
