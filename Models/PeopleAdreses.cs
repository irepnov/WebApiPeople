using System;
using System.Collections.Generic;

namespace PeopleWebApi.Models
{
    public partial class PeopleAdreses
    {
        public long PeopleId { get; set; }
        public long AdresTypeId { get; set; }
        /// <example>адрес</example>
        public string Adres { get; set; }

        public virtual AdresType AdresType { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual People People { get; set; }
    }
}
