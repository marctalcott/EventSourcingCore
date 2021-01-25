using System.Collections.Generic;
using System.Threading.Tasks;
using Ezley.EventStore;

namespace Ezley.EventSourcing
{
    public interface IEventStore
    {
        Task<EventStream> LoadStreamAsyncOrThrowNotFound(string streamId);
        Task<EventStream> LoadStreamAsync(string streamId);

        Task<EventStream> LoadStreamAsync(string streamId, int fromVersion);
  
        Task<bool> AppendToStreamAsync(
            EventUserInfo eventUserInfo, 
            string streamId,
            int expectedVersion,
            IEnumerable<IEvent> events);
    }
}