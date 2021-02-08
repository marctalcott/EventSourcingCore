using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ezley.EventStore;
using Ezley.Shared.Exceptions;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ezley.EventSourcing
{
    public class CosmosDBEventStore : IEventStore
    {
        private readonly IEventTypeResolver _eventTypeResolver;
        private readonly CosmosClient _client;
        private readonly string _databaseId;
        private readonly string _containerId;

        public CosmosDBEventStore(
            IEventTypeResolver eventTypeResolver,
            string endpointUrl, string authorizationKey,
            string databaseId, string containerId)
        {
            _eventTypeResolver = eventTypeResolver;
            _client = new CosmosClient(endpointUrl, authorizationKey);
            _databaseId = databaseId;
            _containerId = containerId;
        }
        public async Task<EventStream> LoadStreamAsyncOrThrowNotFound(string streamId)
        {
            Container container = _client.GetContainer(_databaseId, _containerId);

            var sqlQueryText = $"SELECT * FROM {_containerId} e"
                               + " WHERE e.stream.id = @streamId"
                               + " ORDER BY e.stream.version"; 

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText)
                .WithParameter("@streamId", streamId);

            int version = 0;
            var events = new List<IEvent>();

            FeedIterator<EventWrapper> feedIterator = container.GetItemQueryIterator<EventWrapper>(queryDefinition);
            while (feedIterator.HasMoreResults)
            {
                FeedResponse<EventWrapper> response = await feedIterator.ReadNextAsync();
                foreach (var eventWrapper in response)
                {
                    version = eventWrapper.StreamInfo.Version;

                    events.Add(eventWrapper.GetEvent(_eventTypeResolver));
                }
            }

            if (events.Count == 0)
            {
                throw new NotFoundException();
            }
            
            return new EventStream(streamId, version, events);
        }
        public async Task<EventStream> LoadStreamAsync(string streamId)
        {
            Container container = _client.GetContainer(_databaseId, _containerId);

            var sqlQueryText = $"SELECT * FROM {_containerId} e"
                + " WHERE e.stream.id = @streamId"
                + " ORDER BY e.stream.version"; 

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText)
                .WithParameter("@streamId", streamId);

            int version = 0;
            var events = new List<IEvent>();

            FeedIterator<EventWrapper> feedIterator = container.GetItemQueryIterator<EventWrapper>(queryDefinition);
            while (feedIterator.HasMoreResults)
            {
                FeedResponse<EventWrapper> response = await feedIterator.ReadNextAsync();
                foreach (var eventWrapper in response)
                {
                    version = eventWrapper.StreamInfo.Version;

                    events.Add(eventWrapper.GetEvent(_eventTypeResolver));
                }
            }
            
            return new EventStream(streamId, version, events);
        }

        public async Task<EventStream> LoadStreamAsync(string streamId, int fromVersion)
        {
            Container container = _client.GetContainer(_databaseId, _containerId);

            var sqlQueryText = $"SELECT * FROM {_containerId} e"
                + " WHERE e.stream.id = @streamId AND e.stream.version >= @fromVersion"
                + " ORDER BY e.stream.version"; 

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText)
                .WithParameter("@streamId", streamId)
                .WithParameter("@fromVersion", fromVersion);

            int version = 0;
            var events = new List<IEvent>();

            FeedIterator<EventWrapper> feedIterator = container.GetItemQueryIterator<EventWrapper>(queryDefinition);
            while (feedIterator.HasMoreResults)
            {
                FeedResponse<EventWrapper> response = await feedIterator.ReadNextAsync();
                foreach (var eventWrapper in response)
                {
                    version = eventWrapper.StreamInfo.Version;
                    events.Add(eventWrapper.GetEvent(_eventTypeResolver));
                }
            }

            return new EventStream(streamId, version, events);
        }

        public async Task<bool> AppendToStreamAsync(EventUserInfo eventUserInfo, string streamId, int expectedVersion, IEnumerable<IEvent> events)
        {
            Container container = _client.GetContainer(_databaseId, _containerId);

            PartitionKey partitionKey = new PartitionKey(streamId);

            dynamic[] parameters = new dynamic[]
            {
                streamId,
                expectedVersion,
                SerializeEvents(eventUserInfo, streamId, expectedVersion, events)
            };

            return await container.Scripts.ExecuteStoredProcedureAsync<bool>("spAppendToStream", partitionKey, parameters);
        }
     
        private static string SerializeEvents(EventUserInfo eventUserInfo, string streamId, int expectedVersion, IEnumerable<IEvent> events)
        {
            if (string.IsNullOrEmpty(eventUserInfo.AuthServiceUserId))
                throw new Exception("UserInfo.Id must be set to a value.");
            
            var items = events.Select(e => new EventWrapper
            {
                //Id = $"{streamId}:{++expectedVersion}:{e.GetType().Name}",
                // I'm changing Id because I think there is a flaw in using 
                // stream:version:eventName
                // I had a problem where I found 2 items in the stream with the same version number.
                // After they got added I couldn't add more items because the version number was not 
                // the expected number. It expected the version number to one more the the count of events, 
                // but the count of events was already one more than the last event. The last 2 items on the stream
                // were  like [Guid:12:RenamedFileType, Guid:12:ActivatedFileType].
                // I think this is possible if 2 items hit the SP at the same time and both checked
                // the version number was ok, and then both added. By changing the name to not include the event,
                // I'm guaranteeing that each version of the event has a unique streamId, but that I would 
                // duplicate and fail if 2 items with the same version try to get added at the same time.
                
                Id = $"{streamId}:{++expectedVersion}",
                StreamInfo = new StreamInfo
                {
                    Id = streamId,
                    Version = expectedVersion
                },
                EventType = e.GetType().Name,
                EventData = JObject.FromObject(e),
                UserInfo = JObject.FromObject(eventUserInfo)
            });

            return JsonConvert.SerializeObject(items);
        }

        #region Snapshot Functionality

        private async Task<TSnapshot> LoadSnapshotAsync<TSnapshot>(string streamId)
        {
            Container container = _client.GetContainer(_databaseId, _containerId);

            PartitionKey partitionKey = new PartitionKey(streamId);

            var response = await container.ReadItemAsync<TSnapshot>(streamId, partitionKey);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Resource;
            }

            return default(TSnapshot);
        }

        #endregion
    }
}