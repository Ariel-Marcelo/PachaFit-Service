using System.Linq;
using PACHA_FIT.Core.Domain.User.Dtos;
using EntityUser = PACHA_FIT.Infrastructure.Persistence.Entities.User;

namespace PACHA_FIT.Infrastructure.Repositories.UserFilters;

public class UserUserNameFilter : IUserFilter
{
    public bool CanApply(UserSearchCriteria criteria) => !string.IsNullOrEmpty(criteria.UserName);

    public IQueryable<EntityUser> Apply(IQueryable<EntityUser> query, UserSearchCriteria criteria)
    {
        return query.Where(u => u.FullName != null && u.FullName.Contains(criteria.UserName!));
    }
}
