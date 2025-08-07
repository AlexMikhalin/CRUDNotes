using CRUDNotes.BL.Repositories;
using CRUDNotes.Common;
using CRUDNotes.DAL.Repositories;

namespace CRUDNotes.BL.Services
{
    public class NoteService(ICrudNoteRepository crudNoteRepository)
        : INoteService
    {
        public void CreateNote(NoteDTO dto)
        {
            crudNoteRepository.CreateNote(dto);
        }

        public List<NoteDTO> GetAllNotes()
        {
            return crudNoteRepository.FindAllNotes();
        }

        public void DeleteNote(int id)
        {
           crudNoteRepository.DeleteNote(id);
        }

        public NoteDTO GetNote(int id)
        {
            return crudNoteRepository.FindNote(id);
        }

        public void EditNote(NoteDTO dto)
        {
            crudNoteRepository.EditNote(dto);
        }
    }
}
