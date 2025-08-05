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
                var body = Helper.ObjectToByteArray(dto);

                channel.BasicPublish(exchange: "", routingKey: "notesQueue", basicProperties: null, body: body);
            }
        }
    }
}
