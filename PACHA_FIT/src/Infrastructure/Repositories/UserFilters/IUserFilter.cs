using System.Linq;
using PACHA_FIT.Core.Domain.User.Dtos;
using EntityUser = PACHA_FIT.Infrastructure.Persistence.Entities.User;

namespace PACHA_FIT.Infrastructure.Repositories.UserFilters;

public interface IUserFilter
{
    bool CanApply(UserSearchCriteria criteria);
    IQueryable<EntityUser> Apply(IQueryable<EntityUser> query, UserSearchCriteria criteria);
}
