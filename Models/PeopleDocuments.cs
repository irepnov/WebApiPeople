using System;
using System.Collections.Generic;

namespace PeopleWebApi.Models
{
    public partial class PeopleDocuments
    {
        public long PeopleId { get; set; }
        public long DocumentTypeId { get; set; }
        /// <example>серия</example>
        public string Seria { get; set; }
        /// <example>номер</example>
        public string Number { get; set; }

        public virtual DocumentType DocumentType { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual People People { get; set; }
    }
}
