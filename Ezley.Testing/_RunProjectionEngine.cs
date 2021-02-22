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
                _testConfig.EndpointUri, 
                _testConfig.AuthKey,
                _testConfig.Database,
                _testConfig.ViewContainer);

            var projectionEngine = new CosmosDBProjectionEngine(eventTypeResolver,
                viewRepo,
                _testConfig.EndpointUri, _testConfig.AuthKey, _testConfig.Database,
                _testConfig.EventContainer, _testConfig.LeasesContainer);
            
            projectionEngine.RegisterProjection(new OrderProjection());
            projectionEngine.RegisterProjection(new PendingOrdersProjection());
            
            await projectionEngine.StartAsync("UnitTests");
            await Task.Delay(-1);
        }
    }
}