using Microsoft.AspNetCore.Components.Authorization;

namespace HisuianArchives.AdminPanel.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var anonymous = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity());
            return Task.FromResult(new AuthenticationState(anonymous));
        }
    }
}
