using PACHA_FIT.Core.Domain.Shared;

namespace PACHA_FIT.Core.Domain.User.Specifications;

// TODO: Dont use Entities at Domain Layer
public class UserByEmailSpecification : BaseSpecification<Infrastructure.Persistence.Entities.User>
{
    public UserByEmailSpecification(string email) : base(u => u.Email == email)
    {
        AddInclude(u => u.Role!);
    }
}
