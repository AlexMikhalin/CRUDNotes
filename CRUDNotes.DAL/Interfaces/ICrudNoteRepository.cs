using CRUDNotes.Common;

namespace CRUDNotes.DAL.Repositories
{
    public interface ICrudNoteRepository
    {
        void CreateNote(NoteDTO? dto);
        List<NoteDTO> FindAllNotes();

        void DeleteNote(int id);

        NoteDTO FindNote(int id);

        void EditNote(NoteDTO dto);
    }
}
