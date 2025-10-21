using FluentValidation;

namespace HisuianArchives.Application.Features.Users.Commands.UpdateProfile;

/// <summary>
/// Validator for the UpdateProfileCommand to ensure all required fields are provided and valid.
/// </summary>
public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Bio)
            .MaximumLength(500)
            .WithMessage("Bio cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Bio));
    }
}
