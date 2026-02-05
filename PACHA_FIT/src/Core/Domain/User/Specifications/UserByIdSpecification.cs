using PACHA_FIT.Core.Domain.Shared;

namespace PACHA_FIT.Core.Domain.User.Specifications;

public class UserByIdSpecification : BaseSpecification<Entities.User>
{
    public UserByIdSpecification(int userId) : base(u => u.UserId == userId)
    {
        AddInclude(u => u.Role);
    }
}
