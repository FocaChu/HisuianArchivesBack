using HisuianArchives.Application.DTOs.Auth; 

namespace HisuianArchives.AdminPanel.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("HisuianApi");
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginRequest)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            }

            return null;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}