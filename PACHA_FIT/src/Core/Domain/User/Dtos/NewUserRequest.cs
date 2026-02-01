using System.ComponentModel.DataAnnotations;

namespace PACHA_FIT.Core.Domain.User.Dtos;

public record NewUserRequest(
    [Required, EmailAddress] string Email,
    [Required, MinLength(8)] string Password,
    [Required] string FullName
);