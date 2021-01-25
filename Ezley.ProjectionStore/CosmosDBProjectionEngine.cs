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
        private readonly string _endpointUrl;
        private readonly string _authorizationKey;
        private readonly string _databaseId;
        private readonly string _eventContainerId;
        private readonly string _leaseContainerId;
        private readonly List<IProjection> _projections;
        private ChangeFeedProcessor _changeFeedProcessor;

        public CosmosDBProjectionEngine(IEventTypeResolver eventTypeResolver, IViewRepository viewRepository,
            string endpointUrl, string authorizationKey, string databaseId, string eventContainerId,
            string leaseContainerId)
        {
            _eventTypeResolver = eventTypeResolver;
            _viewRepository = viewRepository;
            _endpointUrl = endpointUrl;
            _authorizationKey = authorizationKey;
            _databaseId = databaseId;
            _eventContainerId = eventContainerId;
            _leaseContainerId = leaseContainerId;
            _projections = new List<IProjection>();
        }

        public void RegisterProjection(IProjection projection)
        {
            _projections.Add(projection);
        }

        public Task StartAsync(string instanceName)
        {
            CosmosClient client = new CosmosClient(_endpointUrl, _authorizationKey);

            Container eventContainer = client.GetContainer(_databaseId, _eventContainerId);
            Container leaseContainer = client.GetContainer(_databaseId, _leaseContainerId);

            _changeFeedProcessor = eventContainer
                .GetChangeFeedProcessorBuilder<Change>("Projection", HandleChangesAsync)
                .WithInstanceName(instanceName)
                .WithLeaseContainer(leaseContainer)
                .WithStartTime(new DateTime(2020, 5, 1, 0, 0, 0, DateTimeKind.Utc))
                .Build();

            return _changeFeedProcessor.StartAsync();
        }

        public Task StopAsync()
        {
            return _changeFeedProcessor.StopAsync();
        }

        private async Task HandleChangesAsync(IReadOnlyCollection<Change> changes, CancellationToken cancellationToken)
        {
            foreach (var change in changes)
            {
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