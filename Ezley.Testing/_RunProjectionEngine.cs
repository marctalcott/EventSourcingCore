using System;
using System.Threading.Tasks;
using Ezley.Events;
using Ezley.Projections;
using Ezley.ProjectionStore;
using Xunit;

namespace Ezley.Testing
{
    public class RunProjectionEngine
    {
        private TestConfig _testConfig = new TestConfig();
        
        [Fact]
        public async Task RegisterProjectionsAsync()
        {
            var eventTypeResolver = new EventTypeResolver();
            var viewRepo = new CosmosDBViewRepository(
                _testConfig.ViewsEndpointUri, 
                _testConfig.ViewsAuthKey,
                _testConfig.ViewsDatabase,
                _testConfig.ViewsContainer);
            
            var projectionEngine = new CosmosDBProjectionEngine(eventTypeResolver, viewRepo,
                _testConfig.EventsEndpointUri, _testConfig.EventsAuthKey, _testConfig.EventsDatabase,
                _testConfig.LeasesEndpointUri, _testConfig.LeasesAuthKey, _testConfig.LeasesDatabase,
                _testConfig.EventContainer, _testConfig.LeasesContainer, _testConfig.StartTimeEpoch);
            
            projectionEngine.RegisterProjection(new OrderProjection());
            projectionEngine.RegisterProjection(new PendingOrdersProjection());
            projectionEngine.RegisterProjection(new CustomerProjection());
            projectionEngine.RegisterProjection(new AllCustomersProjection());
            
            await projectionEngine.StartAsync("UnitTests");
            await Task.Delay(-1);
        }
    }
}