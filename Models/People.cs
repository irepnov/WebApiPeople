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
        /// <example>фамилия</example>
        public string FirstName { get; set; }
        /// <example>имя</example>
        public string MiddleName { get; set; }
        /// <example>отчество</example>
        public string LastName { get; set; }
        /// <example>2020-01-24T07:49:05.564Z</example>
        public DateTime BirthDate { get; set; }

        public virtual ICollection<PeopleAdreses> PeopleAdreses { get; set; }
        public virtual ICollection<PeopleDocuments> PeopleDocuments { get; set; }
    }
}
