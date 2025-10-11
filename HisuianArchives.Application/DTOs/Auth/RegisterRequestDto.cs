using System.ComponentModel.DataAnnotations;

namespace HisuianArchives.Application.DTOs.Auth;

/// <summary>
/// Data Transfer Object for user registration requests.
/// </summary>
public class RegisterRequestDto
{
    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the bio of the user.
    /// </summary>
    [StringLength(500, ErrorMessage = "A biografia não pode exceder 500 caracteres")]
    public string? Bio { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    [StringLength(254, ErrorMessage = "O email não pode exceder 254 caracteres")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    [Required(ErrorMessage = "A senha é obrigatória")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres")]
    public string Password { get; set; } = null!;
}
