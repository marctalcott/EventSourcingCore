using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ezley.EventSourcing;
using Microsoft.Azure.Cosmos;

namespace Ezley.ProjectionStore
{
    public class CosmosDBProjectionEngine : IProjectionEngine
    {
        private readonly IEventTypeResolver _eventTypeResolver;
        private readonly IViewRepository _viewRepository;
        private readonly string _leaseEndpointUrl;
        private readonly string _leaseAuthorizationKey;
        private readonly string _leaseDatabaseId;
        private readonly string _eventEndpointUrl;
        private readonly string _eventAuthorizationKey;
        private readonly string _eventDatabaseId;
        private readonly string _eventContainerId;
        private readonly string _leaseContainerId;
        private readonly List<IProjection> _projections;
        private ChangeFeedProcessor _changeFeedProcessor;
        private long _epochStartTime; // Sets the time (exclusive) to start looking for changes after.
        private string _processorName;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventTypeResolver"></param>
        /// <param name="viewRepository"></param>
        /// <param name="processorName"></param>
        /// <param name="eventEndpointUrl"></param>
        /// <param name="eventAuthorizationKey"></param>
        /// <param name="eventDatabaseId"></param>
        /// <param name="leaseEndpointUrl"></param>
        /// <param name="leaseAuthorizationKey"></param>
        /// <param name="leaseDatabaseId"></param>
        /// <param name="eventContainerId"></param>
        /// <param name="leaseContainerId"></param>
        /// <param name="epochStartTime">Sets the time (exclusive) to start looking for changes after.</param>
        public CosmosDBProjectionEngine(IEventTypeResolver eventTypeResolver, IViewRepository viewRepository,
            string processorName,
            string eventEndpointUrl, string eventAuthorizationKey, string eventDatabaseId,
            string leaseEndpointUrl, string leaseAuthorizationKey, string leaseDatabaseId, string eventContainerId,
            string leaseContainerId, long epochStartTime)
        {
            _eventTypeResolver = eventTypeResolver;
            _viewRepository = viewRepository;
            _processorName = processorName;
            _eventEndpointUrl = eventEndpointUrl;
            _eventAuthorizationKey = eventAuthorizationKey;
            _eventDatabaseId = eventDatabaseId;
            _leaseEndpointUrl = leaseEndpointUrl;
            _leaseAuthorizationKey = leaseAuthorizationKey;
            _leaseDatabaseId = leaseDatabaseId;
            _eventContainerId = eventContainerId;
            _leaseContainerId = leaseContainerId;
            _projections = new List<IProjection>();
            _epochStartTime = epochStartTime;
        }

        public void RegisterProjection(IProjection projection)
        {
            _projections.Add(projection);
        }

        public Task StartAsync(string instanceName)
        {
            CosmosClient eventClient = new CosmosClient(_eventEndpointUrl, _eventAuthorizationKey);
            CosmosClient leaseClient = new CosmosClient(_leaseEndpointUrl, _leaseAuthorizationKey);

            Container eventContainer = eventClient.GetContainer(_eventDatabaseId, _eventContainerId);
            Container leaseContainer = leaseClient.GetContainer(_leaseDatabaseId, _leaseContainerId);

            // start with events at a specific time
            // https://docs.microsoft.com/en-us/azure/cosmos-db/change-feed-processor
            var myTime = DateTimeOffset.FromUnixTimeSeconds(_epochStartTime).UtcDateTime;
            
            _changeFeedProcessor = eventContainer
                .GetChangeFeedProcessorBuilder<Change>(_processorName, HandleChangesAsync)
                .WithInstanceName(instanceName)
                .WithLeaseContainer(leaseContainer)
                .WithStartTime(myTime)
                .Build();

            return _changeFeedProcessor.StartAsync();
        }

        public Task StopAsync()
        {
            return _changeFeedProcessor.StopAsync();
        }

        private async Task HandleChangesAsync(IReadOnlyCollection<Change> changes, CancellationToken cancellationToken)
        {
            // This is needed bc the 'WithStartTime' isn't working for me on the ChangeFeedBuilder
            // This would only be run through the first time after deleting leases container and trying to 
            // replay some events.
            if (changes.First().TimeStamp <= _epochStartTime)
            {
                 changes = changes.Where(x => x.TimeStamp > _epochStartTime)
                    .ToList().AsReadOnly();
            }
            
            foreach (var change in changes)
            {
              //  throw new ApplicationException();
                var @event = change.GetEvent(_eventTypeResolver);

                var subscribedProjections = _projections
                    .Where(projection => projection.IsSubscribedTo(@event));

                foreach (var projection in subscribedProjections)
                {
                    var viewNames = projection.GetViewNames(change.StreamInfo.Id, @event);

                    foreach (var viewName in viewNames)
                    {

                        var handled = false;
                        while (!handled)
                        {
                            var view = await _viewRepository.LoadViewAsync(viewName);

                            // Only update if the LSN of the change is higher than the view. This will ensure
                            // that changes are only processed once.
                            // NOTE: This only works if there's just a single physical partition in Cosmos DB.
                            // TODO: To support multiple partitions we need access to the leases to store
                            // a LSN per lease in the view. This is not yet possible in the V3 SDK.
                            if (view.IsNewerThanCheckpoint(change))
                            {
                                projection.Apply(@event, view);

                                view.UpdateCheckpoint(change);

                                handled = await _viewRepository.SaveViewAsync(viewName, view);
                            }
                            else
                            {
                                // Already handled.
                                handled = true;
                            }

                            if (!handled)
                            {
                                // Oh noos! Somebody changed the view in the meantime, let's wait and try again.
                                await Task.Delay(100);
                            }
                        }
                    }
                }
            }
        }
    }
}