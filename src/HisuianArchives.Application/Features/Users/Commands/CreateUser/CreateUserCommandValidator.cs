using FluentValidation;

namespace HisuianArchives.Application.Features.Users.Commands.CreateUser;

/// <summary>
/// Validator for the CreateUserCommand to ensure all required fields are provided and valid.
/// </summary>
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("A valid email address is required.")
            .MaximumLength(255)
            .WithMessage("Email cannot exceed 255 characters.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100)
            .WithMessage("Password cannot exceed 100 characters.");

        RuleFor(x => x.Bio)
            .MaximumLength(500)
            .WithMessage("Bio cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Bio));
    }
}
