using PACHA_FIT.Core.Domain.Entities;
using PACHA_FIT.Core.Domain.User;

namespace PACHA_FIT.Infrastructure.Repositories;

public static class UserDomainMapper
{
    public static PachaUser? ToDomain(User? user)
    {
        if (user == null) return null;

        return new PachaUser(
            user.UserId,
            user.Email,
            user.FullName,
            user.RoleId,
            user.IsActive,
            user.CreatedAt,
            user.IdentificationType,
            user.IdentificationNumber,
            user.Address,
            user.PhoneNumber,
            user.Role?.Name
        );
    }

    public static void ApplyUpdate(User user, UserUpdateInfo info)
    {
        if (info.Email != null) user.Email = info.Email;
        if (info.FullName != null) user.FullName = info.FullName;
        if (info.IdentificationType != null) user.IdentificationType = info.IdentificationType;
        if (info.IdentificationNumber != null) user.IdentificationNumber = info.IdentificationNumber;
        if (info.Address != null) user.Address = info.Address;
        if (info.PhoneNumber != null) user.PhoneNumber = info.PhoneNumber;
        if (info.RoleId != null) user.RoleId = info.RoleId;
        if (info.IsActive != null) user.IsActive = info.IsActive;
    }
}
