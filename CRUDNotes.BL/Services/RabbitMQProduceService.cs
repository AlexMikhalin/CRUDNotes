using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using CRUDNotes.BL.Repositories;
using CRUDNotes.Common;
using RabbitMQ.Client;

namespace CRUDNotes.BL.Services
{
    public class RabbitMQProduceService : IRabbitMQProduceService
    {
        public void ProduceMessage(NoteDTO dto)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "notesQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                var body = ObjectToByteArray(dto);

                channel.BasicPublish(exchange: "", routingKey: "notesQueue", basicProperties: null, body: body);
            }
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
