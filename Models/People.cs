using System;
using System.Collections.Generic;

namespace PeopleWebApi.Models
{
    public partial class People
    {
        public People()
        {
            PeopleAdreses = new HashSet<PeopleAdreses>();
            PeopleDocuments = new HashSet<PeopleDocuments>();
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public virtual ICollection<PeopleAdreses> PeopleAdreses { get; set; }
        public virtual ICollection<PeopleDocuments> PeopleDocuments { get; set; }
    }
}
