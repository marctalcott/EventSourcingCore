using System;
using System.Threading;
using System.Threading.Tasks;
using Ezley.Events;
using Ezley.Projections;
using Ezley.ProjectionStore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ezley.ProjectionEngine
{
    public class ProjectionBackgroundService: BackgroundService
    {
        private ILogger<ProjectionBackgroundService> _logger;
        private string _endpointUri;
        private string _database;
        private string _authKey;
        private string _eventContainer;
        private string _viewContainer;
        private string _leaseContainer;

        public ProjectionBackgroundService(IConfiguration configuration,
            ILogger<ProjectionBackgroundService> logger)
        {
            _endpointUri = configuration["Azure:EndPointUri"];
            _database = configuration["Azure:Database"];
            _authKey = configuration["Azure:AuthKey"];
            _eventContainer = configuration["Azure:EventContainer"];
            _viewContainer = configuration["Azure:ViewContainer"];
            _leaseContainer = configuration["Azure:LeaseContainer"];
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var eventTypeResolver = new EventTypeResolver();
                var viewRepo = new CosmosDBViewRepository(_endpointUri,_authKey,_database, _viewContainer);

                var projectionEngine = new CosmosDBProjectionEngine(
                    eventTypeResolver, viewRepo,
                    _endpointUri, _authKey, _database, 
                    _eventContainer, _leaseContainer);
                
                projectionEngine.RegisterProjection(new OrderProjection());
                var serviceName = "ProjectionWorkerService";
                await projectionEngine.StartAsync(serviceName);

                _logger.LogInformation($"{serviceName} running at: {DateTimeOffset.Now}");
                await Task.Delay(-1, stoppingToken);
            }
        }

    }
}