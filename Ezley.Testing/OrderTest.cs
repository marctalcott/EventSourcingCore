using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ezley.Events;
using Ezley.EventSourcing;
using Ezley.EventStore;
using Ezley.OrderSystem;
using Ezley.OrderSystem.Repositories;
using Ezley.ProjectionStore;
using Ezley.SnapshotStore;
using Ezley.ValueObjects;
using Xunit;

namespace Ezley.Testing
{
    public class OrderTest
    {
        private int delayMs = 3000;
        private TestConfig _testConfig = new TestConfig();

        [Fact]
        public async Task PlaceOrder()
        {
             var userId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(userId);
            var id = Guid.NewGuid();
            var orderName = "Joey";
            var items = new List<OrderItem>
            {
                new OrderItem(DateTime.Now.ToUniversalTime(),
                    "Ham Sandwich",
                    12.00m),
                new OrderItem(DateTime.Now.ToUniversalTime(),
                    "Clam Chowder",
                    6.59m),
                new OrderItem(DateTime.UtcNow,
                    "French Fries",
                    4.85m)
            };
            var order = new Order(id, orderName, items);
            var repo = GetOrderSystemRepository();
            var saved = await repo.SaveOrder(userInfo, order);
            var order2 = await repo.LoadOrder(id);
            Assert.Equal(id, order2.Id);
            Assert.Equal(orderName, order2.OrderName);
            Assert.Equal(items, order2.Items);

        }
        
        private OrderSystemRepository GetOrderSystemRepository()
        {
            var eventTypeResolver = new EventTypeResolver();
            if (_testConfig.UseInMemoryEventStore)
            {
                var eventStore = new InMemoryEventStore(
                    eventTypeResolver,
                    new Dictionary<string, List<string>>());
                return new OrderSystemRepository(eventStore, null);
            }
            else
            {
                var eventStore = new CosmosDBEventStore(
                    eventTypeResolver,
                    _testConfig.EndpointUri,
                    _testConfig.AuthKey, _testConfig.Database, _testConfig.EventContainer);

                var snapshotStore = new CosmosSnapshotStore(_testConfig.EndpointUri,
                    _testConfig.AuthKey,
                    _testConfig.Database, _testConfig.SnapshotContainer);
                return new OrderSystemRepository(eventStore, snapshotStore);

            }
        }
       
        private CosmosDBViewRepository GetViewRepository()
        {
            return new CosmosDBViewRepository(_testConfig.EndpointUri, _testConfig.AuthKey,
                _testConfig.Database, _testConfig.ViewContainer);
        }
    }
}