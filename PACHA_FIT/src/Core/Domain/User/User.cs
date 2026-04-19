using PACHA_FIT.Core.Domain.User.Dtos;
using PACHA_FIT.Core.Domain.User.Ports;

namespace PACHA_FIT.Core.Domain.User;

public class User
{
    public int UserId { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? FullName { get; set; }
    public int? RoleId { get; set; }
    public bool? IsActive { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public string? IdentificationType { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }

    public User() { }

    public static User CreateFromRegistration(NewUserRegistration registration, IPasswordService passwordService)
    {
        return new User
        {
            Email = registration.Email,
            FullName = registration.FullName,
            PasswordHash = passwordService.HashPassword(registration.Password),
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
