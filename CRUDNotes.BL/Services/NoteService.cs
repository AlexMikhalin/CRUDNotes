using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CRUDNotes.BL.Repositories;
using CRUDNotes.Common;
using CRUDNotes.DAL.Repositories;
using Microsoft.Extensions.Logging;

namespace CRUDNotes.BL.Services
{
    public class NoteService : INoteService
    {
        private readonly ICrudNoteRepository _crudNoteRepository;
        public NoteService(ICrudNoteRepository crudNoteRepository)
        {
            _crudNoteRepository = crudNoteRepository;
        }
        public void CreateNote(NoteDTO dto)
        {
            _crudNoteRepository.CreateNote(dto);
        }

        public List<NoteDTO> GetAllNotes()
        {
            return _crudNoteRepository.FindAllNotes();
        }

        public void DeleteNote(int id)
        {
           _crudNoteRepository.DeleteNote(id);
        }

        public NoteDTO GetNote(int id)
        {
            return _crudNoteRepository.FindNote(id);
        }

        public void EditNote(NoteDTO dto)
        {
            _crudNoteRepository.EditNote(dto);
        }
    }
}
