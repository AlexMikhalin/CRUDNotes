using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using CRUDNotes.BL.Repositories;
using CRUDNotes.Common;
using CRUDNotes.DAL.Repositories;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace CRUDNotes.BL.Services
{
    public class NoteService(ICrudNoteRepository crudNoteRepository, IRabbitMQProduceService rabbitMqProduceService)
        : INoteService
    {
        public void CreateNote(NoteDTO dto)
        {
            // _crudNoteRepository.CreateNote(dto);

            rabbitMqProduceService.ProduceMessage(dto);
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
