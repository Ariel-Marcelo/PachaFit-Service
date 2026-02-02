using System.ComponentModel.DataAnnotations;

namespace PACHA_FIT.Core.Domain.User.Dtos;

public record LoginRequest
{
    [Required(ErrorMessage = "El Nombre de Usuario es obligatorio")]
    public string Username { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La Contraseña es obligatoria")]
    public string Password { get; set; } = string.Empty;
}