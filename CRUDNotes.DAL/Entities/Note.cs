namespace CRUDNotes.DAL.Entities
{
    public class Note
    {
        public int NoteId { get; set; }

        public string Content { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
