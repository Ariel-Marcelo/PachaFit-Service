using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PACHA_FIT.Infrastructure.Api.Dtos;



public class NewUserRequest
{
    [JsonPropertyName("email")]
    [Required(AllowEmptyStrings = true)]
    public string Email { get; set; } = null!;

    [JsonPropertyName("password")]
    [Required]
    [MinLength(8)]
    public string Password { get; set; } = null!;

    [JsonPropertyName("fullName")]
    public string FullName { get; set; } = null!;
}

public class UpdateProfileRequest
{
    [JsonPropertyName("email")]
    [Required(AllowEmptyStrings = true)]
    public string Email { get; set; } = null!;

    [JsonPropertyName("fullName")]
    [Required(AllowEmptyStrings = true)]
    public string FullName { get; set; } = null!;

    [JsonPropertyName("identificationType")]
    public string? IdentificationType { get; set; }

    [JsonPropertyName("identificationNumber")]
    public string? IdentificationNumber { get; set; }

    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }
}

public class UpdateUserRequest
{
    [JsonPropertyName("roleId")]
    public int? RoleId { get; set; }

    [JsonPropertyName("isActive")]
    public bool? IsActive { get; set; }
}

public class ChangePasswordRequest
{
    [JsonPropertyName("currentPassword")]
    [Required(AllowEmptyStrings = false)]
    public string CurrentPassword { get; set; } = null!;

    [JsonPropertyName("newPassword")]
    [Required(AllowEmptyStrings = false)]
    public string NewPassword { get; set; } = null!;
}


