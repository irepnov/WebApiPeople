using System;
using System.Collections.Generic;

namespace PeopleWebApi.Models
{
    public partial class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRoles>();
        }

        public long Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
