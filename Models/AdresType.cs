using System;
using System.Collections.Generic;

namespace PeopleWebApi.Models
{
    public partial class AdresType
    {
        public AdresType()
        {
            PeopleAdreses = new HashSet<PeopleAdreses>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PeopleAdreses> PeopleAdreses { get; set; }
    }
}
