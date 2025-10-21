using FluentValidation;

namespace HisuianArchives.Application.Features.Users.Commands.Login;

/// <summary>
/// Validator for the LoginCommand to ensure all required fields are provided and valid.
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
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
            .MinimumLength(1)
            .WithMessage("Password cannot be empty.")
            .MaximumLength(100)
            .WithMessage("Password cannot exceed 100 characters.");
    }
}
