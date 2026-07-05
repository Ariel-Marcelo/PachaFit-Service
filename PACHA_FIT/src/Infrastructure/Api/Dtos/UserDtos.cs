using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PACHA_FIT.Infrastructure.Api.Dtos;

public class LoginRequest
{
    [JsonPropertyName("username")]
    [Required(AllowEmptyStrings = true)]
    public string Username { get; set; } = null!;

    [JsonPropertyName("password")]
    [Required(AllowEmptyStrings = true)]
    public string Password { get; set; } = null!;
}

public class LoginResponse
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    [JsonPropertyName("fullName")]
    public string? FullName { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; } = null!;

    [JsonPropertyName("roleName")]
    public string? RoleName { get; set; }
}

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


