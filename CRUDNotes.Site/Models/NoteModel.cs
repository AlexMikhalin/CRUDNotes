using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDNotes.Site.Models
{
    public class NoteModel
    {
        public int NoteId { get; set; }

        public string Content { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
