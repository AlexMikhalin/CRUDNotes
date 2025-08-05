using CRUDNotes.Common;

namespace CRUDNotes.BL.Repositories
{
    public interface IRabbitMQProduceService
    {
        void ProduceMessage(NoteDTO dto);
    }
}
