using System;
using System.Collections.Generic;

namespace PeopleWebApi.Models
{
    public partial class PeopleDocuments
    {
        public long PeopleId { get; set; }
        public long DocumentTypeId { get; set; }
        public string Seria { get; set; }
        public string Number { get; set; }

        public virtual DocumentType DocumentType { get; set; }
        public virtual People People { get; set; }
    }
}
