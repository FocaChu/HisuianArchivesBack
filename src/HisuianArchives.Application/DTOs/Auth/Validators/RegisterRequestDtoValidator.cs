using FluentValidation;

namespace HisuianArchives.Application.DTOs.Auth.Validators;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(60).WithMessage("Email cannot exceed 60 characters.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MaximumLength(16).WithMessage("Password cannot exceed 16 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.")
            .NotEqual(x => x.Name).WithMessage("Password cannot be the same as your name.")
            .NotEqual(x => x.Email).WithMessage("Password cannot be the same as your email.");

        RuleFor(x => x.Bio)
            .MaximumLength(160).WithMessage("Bio cannot exceed 160 characters.");
    }
}