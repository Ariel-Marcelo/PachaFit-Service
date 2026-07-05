using System.Linq;
using PACHA_FIT.Core.Domain.User.Dtos;
using EntityUser = PACHA_FIT.Infrastructure.Persistence.Entities.User;

namespace PACHA_FIT.Infrastructure.Repositories.UserFilters;

public class UserEmailFilter : IUserFilter
{
    public bool CanApply(UserSearchCriteria criteria) => !string.IsNullOrEmpty(criteria.Email);

    public IQueryable<EntityUser> Apply(IQueryable<EntityUser> query, UserSearchCriteria criteria)
    {
        return query.Where(u => u.Email == criteria.Email);
    }
}
