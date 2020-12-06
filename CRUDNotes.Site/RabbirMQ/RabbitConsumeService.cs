using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using CRUDNotes.Common;
using CRUDNotes.DAL.Entities;
using CRUDNotes.DAL.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CRUDNotes.Site.RabbirMQ
{
    public class RabbitConsumeService : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private ICrudNoteRepository _crudNoteRepository;

        public RabbitConsumeService(ILoggerFactory loggerFactory, ICrudNoteRepository crudNoteRepository)
        {
            _crudNoteRepository = crudNoteRepository;
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare("notesQueue", false, false, false, null);
        }

        private void ProduceMessage(NoteDTO dto)
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

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                HandleMessage(ea);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("notesQueue", false, consumer);
            return Task.CompletedTask;
        }

        private void HandleMessage(BasicDeliverEventArgs fullPathFile)
        {
            var file = FromByteArray<NoteDTO>(fullPathFile.Body.Span.ToArray());
            _crudNoteRepository.CreateNote(file);
        }

        public T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
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
