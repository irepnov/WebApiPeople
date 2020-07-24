using System;
using System.Collections.Generic;

namespace PeopleWebApi.Models
{
    public partial class DocumentType
    {
        public DocumentType()
        {
            PeopleDocuments = new HashSet<PeopleDocuments>();
        }

        public long Id { get; set; }
        /// <example>тестовый тип документа</example>
        public string Name { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<PeopleDocuments> PeopleDocuments { get; set; }
    }
}
