using System.ComponentModel.DataAnnotations;

namespace PACHA_FIT.Core.Domain.User.Dtos;


public record UpdateUserRequest(
    int? RoleId,
    bool IsActive = true
);