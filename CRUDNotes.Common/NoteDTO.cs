using System;
using System.Collections.Generic;
using System.Text;

namespace CRUDNotes.Common
{
    public class NoteDTO
    {
        public int NoteId { get; set; }

        public string Content { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
