using System;
using System.Collections.Generic;
using System.Linq;
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
using Xunit.Abstractions;

namespace Ezley.Testing
{
    public class OrderTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public OrderTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        private int delayMs = 3000;
        private TestConfig _testConfig = new TestConfig();

        [Fact]
        public async Task PlaceOrder()
        {
            _testOutputHelper.WriteLine($"PlacingOrder");
             var userId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(userId);
            var id = Guid.NewGuid();
            var orderName = "Joey";
            var items = new List<OrderItem>
            {
                new OrderItem(DateTime.UtcNow,
                    "Ham Sandwich",
                    12.00m),
                new OrderItem(DateTime.UtcNow,
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
            Assert.Equal(3, order2.Items.Count);
        }
        
        [Fact]
        public async Task PlaceOrderAndAddItem()
        {
            var userId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(userId);
            var id = Guid.NewGuid();
            var orderName = "Joey";
            var items = new List<OrderItem>
            {
                new OrderItem(DateTime.UtcNow,
                    "Ham Sandwich",
                    12.00m),
                new OrderItem(DateTime.UtcNow,
                    "Clam Chowder",
                    6.59m),
                new OrderItem(DateTime.UtcNow,
                    "French Fries",
                    4.85m)
            };

            var order = new Order(id, orderName, items);
            var repo = GetOrderSystemRepository();

            // add another item:
            var addItem = new OrderItem(DateTime.UtcNow, "Fish Sandwich", 6.95m);
            order.AddItem(addItem);
             var saved = await repo.SaveOrder(userInfo, order);
            // update items so we can use it for comparison
            
            items.Add(addItem);
            var order2 = await repo.LoadOrder(id);
            Assert.Equal(id, order2.Id);
            Assert.Equal(orderName, order2.OrderName);
            Assert.Equal(items, order2.Items);
            Assert.Equal(4, order2.Items.Count);
        }
        [Fact]
        public async Task PlaceOrderAndRemoveItem()
        {
            var userId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(userId);
            var id = Guid.NewGuid();
            var orderName = "Joey";
            var items = new List<OrderItem>
            {
                new OrderItem(DateTime.UtcNow,
                    "Ham Sandwich",
                    12.00m),
                new OrderItem(DateTime.UtcNow,
                    "Clam Chowder",
                    6.59m),
                new OrderItem(DateTime.UtcNow,
                    "French Fries",
                    4.85m)
            };

            var order = new Order(id, orderName, items);
            var repo = GetOrderSystemRepository();

            // remove Item
            var addItem = new OrderItem(DateTime.UtcNow, "Fish Sandwich", 6.95m);
            order.RemoveItem("Clam Chowder");
            var saved = await repo.SaveOrder(userInfo, order);
            // update items so we can use it for comparison

            items.Remove(items.Single(x => x.Name == "Clam Chowder"));
            var order2 = await repo.LoadOrder(id);
            Assert.Equal(id, order2.Id);
            Assert.Equal(orderName, order2.OrderName);
            Assert.Equal(items, order2.Items);
            Assert.Equal(2, order2.Items.Count);
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
                    _testConfig.EventsEndpointUri,
                    _testConfig.EventsAuthKey, _testConfig.EventsDatabase, _testConfig.EventContainer);

                var snapshotStore = new CosmosSnapshotStore(_testConfig.SnapshotsEndpointUri,
                    _testConfig.SnapshotsAuthKey,
                    _testConfig.SnapshotsDatabase, _testConfig.SnapshotsContainer);
                return new OrderSystemRepository(eventStore, snapshotStore);
            }
        }
       
        private CosmosDBViewRepository GetViewRepository()
        {
            return new CosmosDBViewRepository(_testConfig.ViewsEndpointUri, _testConfig.ViewsAuthKey,
                _testConfig.ViewsDatabase, _testConfig.ViewsContainer);
        }
    }
}