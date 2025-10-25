using FluentValidation;

namespace HisuianArchives.Application.Features.Users.Commands.BanUser;

/// <summary>
/// Validator for the BanUserCommand to ensure all required fields are provided and valid.
/// </summary>
public class BanUserCommandValidator : AbstractValidator<BanUserCommand>
{
    public BanUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Ban reason is required.")
            .MaximumLength(500)
            .WithMessage("Ban reason cannot exceed 500 characters.");

        RuleFor(x => x.BannedUntil)
            .GreaterThan(DateTimeOffset.UtcNow)
            .When(x => x.BannedUntil.HasValue)
            .WithMessage("Ban expiration date must be in the future.");
    }
}
