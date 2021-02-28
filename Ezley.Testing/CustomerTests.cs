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
    public class CustomerTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CustomerTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        private int delayMs = 3000;
        private TestConfig _testConfig = new TestConfig();

        [Fact]
        public async Task RegisterCustomer()
        {
            _testOutputHelper.WriteLine($"Registering new customer");
            var eventUserId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(eventUserId);
            
              
            var id = Guid.NewGuid();
            var firstName = $"First_{id}";
            var lastName = $"Last_{id}";
            var middleName = $"Middle_{id}";

            var repo = GetOrderSystemRepository();
            
            var customer = new Customer(id, firstName, lastName, middleName);
            var saved = await repo.SaveCustomer(userInfo, customer);
            var reloadedCustomer= await repo.LoadCustomer(id);
            Assert.Equal(id, reloadedCustomer.Id);
            Assert.Equal(firstName, reloadedCustomer.FirstName);
            Assert.Equal(lastName, reloadedCustomer.LastName);
            Assert.Equal(middleName, reloadedCustomer.MiddleName);
        }
        
        [Fact]
        public async Task RegisterCustomerAndChangeFirstName()
        {
            _testOutputHelper.WriteLine($"Registering new customer");
            var eventUserId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(eventUserId);
            
            var id = Guid.NewGuid();
            var firstName = $"First_{id}";
            var lastName = $"Last_{id}";
            var middleName = $"Middle_{id}";

            var repo = GetOrderSystemRepository();
            
            // things/events occur
            var customer = new Customer(id, firstName, lastName, middleName);
            string newFirstName = firstName + "_changed";
            customer.ChangeFirstName(newFirstName);
            
            var saved = await repo.SaveCustomer(userInfo, customer);
            var reloadedCustomer= await repo.LoadCustomer(id);
            Assert.Equal(id, reloadedCustomer.Id);
            Assert.Equal(newFirstName, reloadedCustomer.FirstName);
            Assert.Equal(lastName, reloadedCustomer.LastName);
            Assert.Equal(middleName, reloadedCustomer.MiddleName);
           
        }
        
        [Fact]
        public async Task RegisterCustomerAndChangeLastName()
        {
            _testOutputHelper.WriteLine($"Registering new customer");
            var eventUserId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(eventUserId);
            
              
            var id = Guid.NewGuid();
            var firstName = $"First_{id}";
            var lastName = $"Last_{id}";
            var middleName = $"Middle_{id}";

            var repo = GetOrderSystemRepository();
            
            // things/events occur
            var customer = new Customer(id, firstName, lastName, middleName);
            string newLastName = lastName + "_changed";
            customer.ChangeLastName(newLastName);
            
            var saved = await repo.SaveCustomer(userInfo, customer);
            var reloadedCustomer= await repo.LoadCustomer(id);
            Assert.Equal(id, reloadedCustomer.Id);
            Assert.Equal(firstName, reloadedCustomer.FirstName);
            Assert.Equal(newLastName, reloadedCustomer.LastName);
            Assert.Equal(middleName, reloadedCustomer.MiddleName);
        }
        
        [Fact]
        public async Task RegisterCustomerAndChangeMiddleName()
        {
            _testOutputHelper.WriteLine($"Registering new customer");
            var eventUserId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(eventUserId);
            
              
            var id = Guid.NewGuid();
            var firstName = $"First_{id}";
            var lastName = $"Last_{id}";
            var middleName = $"Middle_{id}";

            var repo = GetOrderSystemRepository();
            
            // things/events occur
            var customer = new Customer(id, firstName, lastName, middleName);
            string newMiddleName = middleName + "_changed";
            customer.ChangeMiddleName(newMiddleName);
            
            var saved = await repo.SaveCustomer(userInfo, customer);
            var reloadedCustomer= await repo.LoadCustomer(id);
            Assert.Equal(id, reloadedCustomer.Id);
            Assert.Equal(firstName, reloadedCustomer.FirstName);
            Assert.Equal(lastName, reloadedCustomer.LastName);
            Assert.Equal(newMiddleName, reloadedCustomer.MiddleName);
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
            return new CosmosDBViewRepository(_testConfig.CustomerViewsEndpointUri, _testConfig.CustomerViewsAuthKey,
                _testConfig.CustomerViewsDatabase, _testConfig.CustomerViewsContainer);
        }
        
        
    }
}