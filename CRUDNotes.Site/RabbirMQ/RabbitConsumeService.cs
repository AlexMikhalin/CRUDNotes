using CRUDNotes.Common;
using CRUDNotes.DAL.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CRUDNotes.Site.RabbirMQ
{
    public class RabbitConsumeService : BackgroundService
    {
        private readonly IModel _channel;
        private readonly ICrudNoteRepository _crudNoteRepository;

        public RabbitConsumeService(ICrudNoteRepository crudNoteRepository)
        {
            _crudNoteRepository = crudNoteRepository;

            var factory = new ConnectionFactory { HostName = "localhost" };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

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
