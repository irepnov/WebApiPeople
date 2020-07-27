using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeopleWebApi.Models
{
    public partial class AdresType
    {
        public AdresType()
        {
            PeopleAdreses = new HashSet<PeopleAdreses>();
        }

        public long Id { get; set; }
        /// <example>тестовый тип адреса</example>
        [MaxLength(25)]
        public string Name { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<PeopleAdreses> PeopleAdreses { get; set; }
    }
}
