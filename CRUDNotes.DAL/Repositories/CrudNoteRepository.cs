using CRUDNotes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using CRUDNotes.DAL.EF;
using CRUDNotes.DAL.Entities;

namespace CRUDNotes.DAL.Repositories
{
    public class CrudNoteRepository : ICrudNoteRepository
    {
        public DataBaseContext _db;

        public CrudNoteRepository(DataBaseContext context)
        {
            _db = context;
        }
        public void CreateNote(NoteDTO dto)
        {
            _db.Notes.Add(new Note() {Content = dto.Content, CreateDate = DateTime.Now});
            _db.SaveChanges();
        }

        public List<NoteDTO> FindAllNotes()
        {
            var allNotes = _db.Notes.ToList();
            List<NoteDTO> resultList = new List<NoteDTO>();

            foreach (var n in allNotes)
            {
                resultList.Add(new NoteDTO(){ Content = n.Content, CreateDate = n.CreateDate, NoteId = n.NoteId });
            }

            return resultList;
        }

        public void DeleteNote(int id)
        {
            Note note = _db.Notes.FirstOrDefault(n => n.NoteId == id);
            if (note != null)
            {
                _db.Notes.Remove(note);
                _db.SaveChanges();
            }
        }

        public NoteDTO FindNote(int id)
        {
            Note noteEntity = _db.Notes.FirstOrDefault(p => p.NoteId == id);
            if (noteEntity !=null)
                 return new NoteDTO(){ Content = noteEntity.Content, NoteId = noteEntity.NoteId, CreateDate = noteEntity.CreateDate };
            return null;
        }

        public void EditNote(NoteDTO dto)
        {
            Note note = _db.Notes.FirstOrDefault(n => n.NoteId == dto.NoteId);
            if (note != null)
            {
                note.Content = dto.Content;
                note.CreateDate = DateTime.Now;
                _db.Notes.Update(note);
                _db.SaveChanges();
            }
        }
    }
}
