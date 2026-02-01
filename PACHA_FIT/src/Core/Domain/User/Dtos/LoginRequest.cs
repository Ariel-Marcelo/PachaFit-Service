using System.ComponentModel.DataAnnotations;

namespace PACHA_FIT.Core.Domain.User.Dtos;

public record LoginRequest
{
    [Required(ErrorMessage = "El usuario es obligatorio")]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 20 caracteres")]
    public string Password { get; set; } = string.Empty;
}