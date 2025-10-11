using System.ComponentModel.DataAnnotations;

namespace HisuianArchives.Application.DTOs.Auth;

/// <summary>
/// Data Transfer Object for user login requests.
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user's password.
    /// </summary>
    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Password { get; set; } = null!;
}
