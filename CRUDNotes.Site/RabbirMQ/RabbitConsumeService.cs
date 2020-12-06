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

            var factory = new ConnectionFactory { HostName = "localhost" };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare("notesQueue", false, false, false, null);
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
            _crudNoteRepository.CreateNote(Helper.FromByteArray<NoteDTO>(fullPathFile.Body.Span.ToArray()));
        }
    }
}
