using System;
using System.Collections.Generic;

namespace PeopleWebApi.Models
{
    public partial class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRoles>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
