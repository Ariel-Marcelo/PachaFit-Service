using System.ComponentModel.DataAnnotations;

namespace PACHA_FIT.Core.Domain.User.Dtos;

public record LoginRequest
{
    [Required(ErrorMessage = "El Nombre de Usuario es obligatorio", AllowEmptyStrings = false)]
    public string Username { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La Contraseña es obligatoria", AllowEmptyStrings = false)]
    public string Password { get; set; } = string.Empty;
}