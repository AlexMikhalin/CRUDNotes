using CRUDNotes.Common;

namespace CRUDNotes.BL.Repositories
{
    public interface INoteService
    {
        void CreateNote(NoteDTO dto);

        List<NoteDTO> GetAllNotes();

        void DeleteNote(int id);

        NoteDTO GetNote(int id);

        void EditNote(NoteDTO dto);
    }
}
