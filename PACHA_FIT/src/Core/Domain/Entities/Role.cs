using System;
using System.Collections.Generic;
using PACHA_FIT.Core.Domain.Entities;

namespace PACHA_FIT.src.Core.Domain.Entities;

public partial class Role
{
    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
