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
            var orderViewRepo = new CosmosDBViewRepository(
                _testConfig.OrderViewsEndpointUri, 
                _testConfig.OrderViewsAuthKey,
                _testConfig.OrderViewsDatabase,
                _testConfig.OrderViewsContainer);
            var orderProcessorName = "OrderUnitTestsProcessor";
            var orderProjectionEngine = new CosmosDBProjectionEngine(eventTypeResolver, orderViewRepo,orderProcessorName,
                _testConfig.EventsEndpointUri, _testConfig.EventsAuthKey, _testConfig.EventsDatabase,
                _testConfig.LeasesEndpointUri, _testConfig.LeasesAuthKey, _testConfig.LeasesDatabase,
                _testConfig.EventContainer, _testConfig.LeasesContainer, _testConfig.StartTimeEpoch);
            
            orderProjectionEngine.RegisterProjection(new OrderProjection());
            orderProjectionEngine.RegisterProjection(new PendingOrdersProjection());
            await orderProjectionEngine.StartAsync("UnitTests");
            // ---
            var customerViewRepo = new CosmosDBViewRepository(
                _testConfig.CustomerViewsEndpointUri, 
                _testConfig.CustomerViewsAuthKey,
                _testConfig.CustomerViewsDatabase,
                _testConfig.CustomerViewsContainer);
            var customerProcessorName = "CustomerUnitTestsProcessor";
            var customerProjectionEngine = new CosmosDBProjectionEngine(eventTypeResolver, customerViewRepo,customerProcessorName,
                _testConfig.EventsEndpointUri, _testConfig.EventsAuthKey, _testConfig.EventsDatabase,
                _testConfig.LeasesEndpointUri, _testConfig.LeasesAuthKey, _testConfig.LeasesDatabase,
                _testConfig.EventContainer, _testConfig.LeasesContainer, _testConfig.StartTimeEpoch);
            
            customerProjectionEngine.RegisterProjection(new CustomerProjection());
            customerProjectionEngine.RegisterProjection(new AllCustomersProjection());
            
            await customerProjectionEngine.StartAsync("UnitTests");
            await Task.Delay(-1);
        }
        
    }
}