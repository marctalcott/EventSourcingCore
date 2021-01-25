using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ezley.EventStore;
using Ezley.Shared.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ezley.EventSourcing
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly IEventTypeResolver _eventTypeResolver;
        private readonly Dictionary<string, List<string>> _eventsContainer;

        public InMemoryEventStore(
            IEventTypeResolver eventTypeResolver,
            Dictionary<string, List<string>> eventsContainer)

            {
                    _eventTypeResolver = eventTypeResolver;
                    _eventsContainer = eventsContainer;
            }
        public async Task<EventStream> LoadStreamAsyncOrThrowNotFound(string streamId)
        {
            var eventWrappers = await LoadOrderedEventWrappers(streamId);
            if (eventWrappers.Count == 0)
            {
                throw new NotFoundException();
            }
            
            int version = eventWrappers.Max(x => x.StreamInfo.Version);
            var events = new List<IEvent>();
            foreach (var wrapper in eventWrappers)
            {
                events.Add(wrapper.GetEvent(_eventTypeResolver));
            }
            return new EventStream(streamId, version, events);
        }
        
        public async Task<EventStream> LoadStreamAsync(string streamId)
        {
            var eventWrappers = await LoadOrderedEventWrappers(streamId);

            int version = eventWrappers.Count > 0
                ? eventWrappers.Max(x => x.StreamInfo.Version)
                : 0;
            var events = new List<IEvent>();
            foreach (var wrapper in eventWrappers)
            {
                events.Add(wrapper.GetEvent(_eventTypeResolver));
            }
            return new EventStream(streamId, version, events);
        }
       
        public async Task<EventStream> LoadStreamAsync(string streamId, int fromVersion)
        {
            var eventWrappers = await LoadOrderedEventWrappersFromVersion(streamId, fromVersion);
            
            if (eventWrappers.Count == 0)
            {
                throw new NotFoundException();
            }
            
            int version = eventWrappers.Max(x => x.StreamInfo.Version);
            var events = new List<IEvent>();
            foreach (var wrapper in eventWrappers)
            {
                events.Add(wrapper.GetEvent(_eventTypeResolver));
            }
            return new EventStream(streamId, version, events);
           
        }
        private async Task<List<EventWrapper>> LoadOrderedEventWrappers(string streamId)
        {
            List<string> eventData = _eventsContainer.ContainsKey(streamId)
                ? _eventsContainer[streamId]
                : new List<string>();
            
            var eventWrappers = new List<EventWrapper>();
            
            foreach (var data in eventData)
            {
                var eventWrapper = JsonConvert.DeserializeObject<EventWrapper>(data);
                eventWrappers.Add(eventWrapper);
            }
 
            eventWrappers = eventWrappers.OrderBy(x => x.StreamInfo.Version).ToList();
            return eventWrappers;
        }
        private async Task<List<EventWrapper>> LoadOrderedEventWrappersFromVersion(string streamId, int version)
        {
            List<string> eventData =
                _eventsContainer.ContainsKey(streamId)
                    ? _eventsContainer[streamId]
                    : new List<string>();
            var eventWrappers = new List<EventWrapper>();
            
            foreach (var data in eventData)
            {
                var eventWrapper = JsonConvert.DeserializeObject<EventWrapper>(data);
                if (eventWrapper.StreamInfo.Version >= version)
                {
                    eventWrappers.Add(eventWrapper);
                }
            }
 
            eventWrappers = eventWrappers.OrderBy(x => x.StreamInfo.Version).ToList();
            return eventWrappers;
        }
        public async Task<bool> AppendToStreamAsync(EventUserInfo eventUserInfo, string streamId, int expectedVersion, IEnumerable<IEvent> events)
        {
            var lockObject = new object();
            lock(lockObject)
            {
                // Load stream and verify version hasn't been changed yet.
                var eventStream = LoadStreamAsync(streamId).GetAwaiter().GetResult();

                if (eventStream.Version != expectedVersion)
                {
                    return false;
                }

                var wrappers = PrepareEvents(eventUserInfo, streamId, expectedVersion, events);
                var stream = _eventsContainer.ContainsKey(streamId)
                    ? this._eventsContainer[streamId]
                    : new List<string>();
                
                foreach (var wrapper in wrappers)
                {
                     stream.Add(JsonConvert.SerializeObject(wrapper));
                }

                if (!_eventsContainer.ContainsKey(streamId))
                {
                    _eventsContainer.Add(streamId, stream);
                }
                else
                {
                    _eventsContainer[streamId] = stream;
                }
            }
            return true;
        }
     
        private static List<EventWrapper> PrepareEvents(EventUserInfo eventUserInfo, string streamId, int expectedVersion, IEnumerable<IEvent> events)
        {
            if (string.IsNullOrEmpty(eventUserInfo.AuthServiceUserId))
                throw new Exception("UserInfo.Id must be set to a value.");
            
            var items = events.Select(e => new EventWrapper
            {
                Id = $"{streamId}:{++expectedVersion}:{e.GetType().Name}",
                StreamInfo = new StreamInfo
                {
                    Id = streamId,
                    Version = expectedVersion
                },
                EventType = e.GetType().Name,
                EventData = JObject.FromObject(e),
                UserInfo = JObject.FromObject(eventUserInfo)
            });

            return items.ToList();
        }

        #region Snapshot Functionality

        // private async Task<TSnapshot> LoadSnapshotAsync<TSnapshot>(string streamId)
        // {
        //     //Container container = _client.GetContainer(_databaseId, _containerId);
        //
        //     PartitionKey partitionKey = new PartitionKey(streamId);
        //
        //     var response = await container.ReadItemAsync<TSnapshot>(streamId, partitionKey);
        //     if (response.StatusCode == HttpStatusCode.OK)
        //     {
        //         return response.Resource;
        //     }
        //
        //     return default(TSnapshot);
        // }

        #endregion
    }
}