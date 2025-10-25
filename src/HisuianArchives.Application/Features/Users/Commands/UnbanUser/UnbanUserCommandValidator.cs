using FluentValidation;

namespace HisuianArchives.Application.Features.Users.Commands.UnbanUser;

/// <summary>
/// Validator for the UnbanUserCommand to ensure all required fields are provided and valid.
/// </summary>
public class UnbanUserCommandValidator : AbstractValidator<UnbanUserCommand>
{
    public UnbanUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");
    }
}
