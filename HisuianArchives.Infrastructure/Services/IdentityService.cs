using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HisuianArchives.Infrastructure.Services;

/// <summary>
/// Implementation of <see cref="IIdentityService"/> that uses the current request's <see cref="HttpContext"/>
/// to extract information about the authenticated user.
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _contextAccessor;

    /// <summary>
    /// Constructor that injects the <see cref="IHttpContextAccessor"/>.
    /// </summary>
    /// <param name="contextAccessor">Accessor for the current request's <see cref="HttpContext"/>.</param>
    public IdentityService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    /// <summary>
    /// Gets the user ID of the current session.
    /// </summary>
    /// <returns>The user's ID as a string, or null if the user is not authenticated.</returns>
    public string? GetUserId()
    {
        return _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}