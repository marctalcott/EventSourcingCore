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
        private string _eventsEndpointUri;
        private string _eventsDatabase;
        private string _eventsAuthKey;
        private string _eventsContainer;
        
        private string _viewsEndpointUri;
        private string _viewsDatabase;
        private string _viewsAuthKey;
        private string _viewsContainer;
        
        private string _leasesEndpointUri;
        private string _leasesDatabase;
        private string _leasesAuthKey;
        private string _leasesContainer;

        private long _startDateTimeUtcEpochSeconds;
        private DateTime _startDateTimeUtc;
        public ProjectionBackgroundService(IConfiguration configuration,
            ILogger<ProjectionBackgroundService> logger)
        {
            _eventsEndpointUri = configuration["Azure:EventsEndPointUri"];
            _eventsDatabase = configuration["Azure:EventsDatabase"];
            _eventsAuthKey = configuration["Azure:EventsAuthKey"];
            _eventsContainer = configuration["Azure:EventsContainer"];
            
            _viewsEndpointUri = configuration["Azure:ViewsEndPointUri"];
            _viewsDatabase = configuration["Azure:ViewsDatabase"];
            _viewsAuthKey = configuration["Azure:ViewsAuthKey"];
            _viewsContainer = configuration["Azure:ViewsContainer"];
            
            _leasesEndpointUri = configuration["Azure:LeasesEndPointUri"];
            _leasesDatabase = configuration["Azure:LeasesDatabase"];
            _leasesAuthKey = configuration["Azure:LeasesAuthKey"];
            _leasesContainer = configuration["Azure:LeasesContainer"];

            // get datetime from epoch seconds
            _startDateTimeUtcEpochSeconds = long.Parse(configuration["Azure:ProjectionStartTimeUtcExclusive"]);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            _startDateTimeUtc = epoch.AddSeconds(_startDateTimeUtcEpochSeconds);
            
            _logger = logger;
        }
       
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var eventTypeResolver = new EventTypeResolver();
                var viewRepo = new CosmosDBViewRepository(_viewsEndpointUri,
                    _viewsAuthKey, _viewsDatabase, _viewsContainer);

                
                var projectionEngine = new CosmosDBProjectionEngine(
                    eventTypeResolver, viewRepo,
                    _eventsEndpointUri, _eventsAuthKey, _eventsDatabase, 
                    _leasesEndpointUri, _leasesAuthKey, _leasesDatabase, 
                    _eventsContainer,_leasesContainer, _startDateTimeUtc
                    );
                
                projectionEngine.RegisterProjection(new OrderProjection());
                projectionEngine.RegisterProjection(new PendingOrdersProjection());
                
                var serviceName = "ProjectionWorkerService";
                await projectionEngine.StartAsync(serviceName);

                _logger.LogInformation($"{serviceName} running at: {DateTimeOffset.Now}");
                await Task.Delay(-1, stoppingToken);
            }
        }

    }
}