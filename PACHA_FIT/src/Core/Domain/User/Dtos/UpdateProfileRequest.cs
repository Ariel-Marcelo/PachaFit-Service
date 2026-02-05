using System.ComponentModel.DataAnnotations;

namespace PACHA_FIT.Core.Domain.User.Dtos;

public record UpdateProfileRequest(
    [property: Required(ErrorMessage = "El Email es obligatorio"), EmailAddress(ErrorMessage = "El Email no es válido")] string Email,
    [property: Required(ErrorMessage = "El Nombre de Usuario es obligatorio")] string FullName,
    string? IdentificationType,
    string? IdentificationNumber,
    string? Address,
    string? PhoneNumber
);