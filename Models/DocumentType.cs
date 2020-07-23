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
        public string Name { get; set; }

        public virtual ICollection<PeopleDocuments> PeopleDocuments { get; set; }
    }
}
