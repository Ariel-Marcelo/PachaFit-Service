using System.Linq;
using PACHA_FIT.Core.Domain.User.Dtos;
using EntityUser = PACHA_FIT.Infrastructure.Persistence.Entities.User;

namespace PACHA_FIT.Infrastructure.Repositories.UserFilters;

public class UserIdFilter : IUserFilter
{
    public bool CanApply(UserSearchCriteria criteria) => criteria.UserId.HasValue;

    public IQueryable<EntityUser> Apply(IQueryable<EntityUser> query, UserSearchCriteria criteria)
    {
        return query.Where(u => u.UserId == criteria.UserId!.Value);
    }
}
