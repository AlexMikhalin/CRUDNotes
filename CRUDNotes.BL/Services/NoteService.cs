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
    public class NoteService : INoteService
    {
        private readonly ICrudNoteRepository _crudNoteRepository;
        private readonly IRabbitMQProduceService _rabbitMqProduceService;
        public NoteService(ICrudNoteRepository crudNoteRepository, IRabbitMQProduceService rabbitMqProduceService)
        {
            _crudNoteRepository = crudNoteRepository;
            _rabbitMqProduceService = rabbitMqProduceService;
        }
        public void CreateNote(NoteDTO dto)
        {
            // _crudNoteRepository.CreateNote(dto);

            //var factory = new ConnectionFactory() { HostName = "localhost" };
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    channel.QueueDeclare(queue: "notesQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            //    var body = ObjectToByteArray(dto);

            //    channel.BasicPublish(exchange: "", routingKey: "notesQueue", basicProperties: null, body: body);
            //}

            _rabbitMqProduceService.ProduceMessage(dto);
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

        byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}
