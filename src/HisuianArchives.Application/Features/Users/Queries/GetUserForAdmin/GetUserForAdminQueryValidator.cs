using FluentValidation;

namespace HisuianArchives.Application.Features.Users.Queries.GetUserForAdmin;

/// <summary>
/// Validator for the GetUserForAdminQuery to ensure at least one search parameter is provided.
/// </summary>
public class GetUserForAdminQueryValidator : AbstractValidator<GetUserForAdminQuery>
{
    public GetUserForAdminQueryValidator()
    {
        RuleFor(x => x)
            .Must(x => x.UserId.HasValue || !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Either UserId or Email must be provided.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("A valid email address is required when searching by email.");
    }
}
