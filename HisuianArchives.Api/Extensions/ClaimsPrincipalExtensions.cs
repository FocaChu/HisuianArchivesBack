using System.Security.Claims;

namespace HisuianArchives.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetCurrentUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new InvalidOperationException("User ID claim is missing or invalid in the token.");
        }

        return userId;
    }
}