using System.ComponentModel.DataAnnotations;

namespace PACHA_FIT.Core.Domain.User.Dtos;

public record UpdateUserRequest(
    string UserUuid,
    [property: Required(ErrorMessage = "El Email es obligatorio"), EmailAddress(ErrorMessage = "El Email no es válido")] string Email,
    [property: Required, MinLength(8, ErrorMessage = "Password máximo 8 caractéres")] string Password,
    [property: Required(ErrorMessage = "El Nombre de Usuario es obligatorio")] string FullName,
    string IdentificationType,
    string IdentificationNumber,
    string Address,
    string PhoneNumber,
    string RoleId
);