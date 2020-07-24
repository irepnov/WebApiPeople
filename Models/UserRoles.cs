using System;
using System.Collections.Generic;

namespace PeopleWebApi.Models
{
    public partial class UserRoles
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
