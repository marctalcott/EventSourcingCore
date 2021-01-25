// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Ezley.Domain.CRM;
// using Ezley.Domain.CRM.Repositories;
// using Ezley.Events;
// using Ezley.EventSourcing;
// using Ezley.EventSourcing.Shared;
// using Ezley.SnapshotStore;
// using Ezley.ValueObjects;
// using Xunit;
//
// namespace Ezley.Testing
// {
//     public class UserTests
//     {
//         private readonly TestConfig _testConfig = new TestConfig();
//
//         [Fact]
//         public async Task RegisterUser()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var tenantId = Guid.NewGuid();
//             var personName = new PersonName("Joe", "Blow");
//             var displayName = "Joey Blowie";
//             var address = new Address("123 Main St.", "Suite 313", "Box 3", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//            
//             // Aggregate object function triggers an event on aggregate
//             var user = new User(id, tenantId,personName, displayName, address, phone, email);
//
//             var repository = GetRepository();
//
//             // Save aggregate. Persists the events created.
//             var success = await repository.SaveUser(userInfo, user);
//             Assert.True(success);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var user2 = await repository.LoadUser(id);
//             Assert.Equal(id, user2.Id);
//             Assert.Equal(tenantId, user2.TenantId);
//             Assert.Equal(personName, user2.PersonName);
//             Assert.Equal(displayName, user2.DisplayName);
//             Assert.True(Equals(address, user2.Address));
//             Assert.True(Equals(phone,  user2.Phone));
//             Assert.True(Equals(email,user2.Email));
//             Assert.True(user2.Active);
//         }
//         
//         [Fact]
//         public async Task User_ChangeTenantId()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var tenantId = Guid.Empty;
//             var personName = new PersonName("Joe", "Blow");
//             var displayName = "Joey Blowie";
//             var address = new Address("123 Main St.", "Suite 313", "Box 3", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//            
//             // Aggregate object function triggers an event on aggregate
//             var user = new User(id, tenantId, personName, displayName, address, phone, email);
//
//             var tenantId2 = Guid.NewGuid();
//             user.ChangeTenantId(tenantId2);
//             
//             var repository = GetRepository();
//             var success = await repository.SaveUser(userInfo, user);
//             Assert.True(success);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var user2 = await repository.LoadUser(id);
//             Assert.Equal(id, user2.Id);
//             Assert.Equal(tenantId2, user2.TenantId);
//             Assert.Equal(personName, user2.PersonName);
//             Assert.Equal(displayName, user2.DisplayName);
//             Assert.True(Equals(address, user2.Address));
//             Assert.True(Equals(phone,  user2.Phone));
//             Assert.True(Equals(email,user2.Email));
//             Assert.True(user2.Active);
//         }
//         
//         [Fact]
//         public async Task User_ChangePersonName()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var tenantId = Guid.NewGuid();
//             var personName = new PersonName("Joe", "Blow");
//             var displayName = "Joey Blowie";
//             var address = new Address("123 Main St.", "Suite 313", "Box 3", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//            
//             // Aggregate object function triggers an event on aggregate
//             var user = new User(id, tenantId,personName, displayName, address, phone, email);
//             
//             var personName2 = new PersonName("Janet", "Smith");
//             user.ChangePersonName(personName2);
//             
//             var repository = GetRepository();
//             var success = await repository.SaveUser(userInfo, user);
//             Assert.True(success);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var user2 = await repository.LoadUser(id);
//             Assert.Equal(id, user2.Id);
//             Assert.Equal(tenantId, user2.TenantId);
//             Assert.Equal(personName2, user2.PersonName);
//             Assert.Equal(displayName, user2.DisplayName);
//             Assert.True(Equals(address, user2.Address));
//             Assert.True(Equals(phone,  user2.Phone));
//             Assert.True(Equals(email,user2.Email));
//             Assert.True(user2.Active);
//         }
//         
//         [Fact]
//         public async Task User_ChangeDisplayName()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var tenantId = Guid.NewGuid();
//             var personName = new PersonName("Joe", "Blow");
//             var displayName = "Joey Blowie";
//             var address = new Address("123 Main St.", "Suite 313", "Box 3", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//            
//             // Aggregate object function triggers an event on aggregate
//             var user = new User(id, tenantId,personName, displayName, address, phone, email);
//
//             var displayName2 = "Jo";
//             user.ChangeDisplayName(displayName2);
//             
//             var repository = GetRepository();
//             var success = await repository.SaveUser(userInfo, user);
//             Assert.True(success);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var user2 = await repository.LoadUser(id);
//             Assert.Equal(id, user2.Id);
//             Assert.Equal(tenantId, user2.TenantId);
//             Assert.Equal(personName, user2.PersonName);
//             Assert.Equal(displayName2, user2.DisplayName);
//             Assert.True(Equals(address, user2.Address));
//             Assert.True(Equals(phone,  user2.Phone));
//             Assert.True(Equals(email,user2.Email));
//             Assert.True(user2.Active);
//         }
//
//         [Fact]
//         public async Task User_ChangeAddress()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var tenantId = Guid.NewGuid();
//             var personName = new PersonName("Joe", "Blow");
//             var displayName = "Joey Blowie";
//             var address = new Address("123 Main St.", "Suite 313", "Box 3", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//            
//             // Aggregate object function triggers an event on aggregate
//             var user = new User(id, tenantId,personName, displayName, address, phone, email);
//
//             var address2 = new Address("Line1 -2", "Line2 -2", "Line3 -3", 
//                 "MyCity", "55555", "WO", "USA");
//             user.ChangeAddress(address2);
//             
//             var repository = GetRepository();
//             var success = await repository.SaveUser(userInfo, user);
//             Assert.True(success);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var user2 = await repository.LoadUser(id);
//             Assert.Equal(id, user2.Id);
//             Assert.Equal(tenantId, user2.TenantId);
//             Assert.Equal(personName, user2.PersonName);
//             Assert.Equal(displayName, user2.DisplayName);
//             Assert.True(Equals(address2, user2.Address));
//             Assert.True(Equals(phone,  user2.Phone));
//             Assert.True(Equals(email,user2.Email));
//             Assert.True(user2.Active);
//         }
//         
//         [Fact]
//         public async Task User_ChangeEmail2()
//         { 
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var tenantId = Guid.NewGuid();
//             var personName = new PersonName("Joe", "Blow");
//             var displayName = "Joey Blowie";
//             var address = new Address("123 Main St.", "Suite 313", "Box 3", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//            
//             // Aggregate object function triggers an event on aggregate
//             var user = new User(id, tenantId,personName, displayName, address, phone, email);
//
//             var email2 = new Email("MyNewAdd@msn.com");
//             user.ChangeEmail(email2);
//             
//             var repository = GetRepository();
//             var success = await repository.SaveUser(userInfo, user);
//             Assert.True(success);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var user2 = await repository.LoadUser(id);
//             Assert.Equal(id, user2.Id);
//             Assert.Equal(tenantId, user2.TenantId);
//             Assert.Equal(personName, user2.PersonName);
//             Assert.Equal(displayName, user2.DisplayName);
//             Assert.True(Equals(address, user2.Address));
//             Assert.True(Equals(phone,  user2.Phone));
//             Assert.True(Equals(email2,user2.Email));
//             Assert.True(user2.Active);
//         }
//         
//         [Fact]
//         public async Task User_ChangePhone()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var tenantId = Guid.NewGuid();
//             var personName = new PersonName("Joe", "Blow");
//             var displayName = "Joey Blowie";
//             var address = new Address("123 Main St.", "Suite 313", "Box 3", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//            
//             // Aggregate object function triggers an event on aggregate
//             var user = new User(id, tenantId,personName, displayName, address, phone, email);
//
//             var phone2 = new Phone("(444)333-2221");
//             user.ChangePhone(phone2);
//             
//             var repository = GetRepository();
//             var success = await repository.SaveUser(userInfo, user);
//             Assert.True(success);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var user2 = await repository.LoadUser(id);
//             Assert.Equal(id, user2.Id);
//             Assert.Equal(tenantId, user2.TenantId);
//             Assert.Equal(personName, user2.PersonName);
//             Assert.Equal(displayName, user2.DisplayName);
//             Assert.True(Equals(address, user2.Address));
//             Assert.True(Equals(phone2,  user2.Phone));
//             Assert.True(Equals(email,user2.Email));
//             Assert.True(user2.Active);
//         }
//         
//         [Fact]
//         public async Task User_Deactivate()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var tenantId = Guid.NewGuid();
//             var personName = new PersonName("Joe", "Blow");
//             var displayName = "Joey Blowie";
//             var address = new Address("123 Main St.", "Suite 313", "Box 3", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//            
//             // Aggregate object function triggers an event on aggregate
//             var user = new User(id, tenantId,personName, displayName, address, phone, email);
//             
//             user.Deactivate();
//             
//             var repository = GetRepository();
//             var success = await repository.SaveUser(userInfo, user);
//             Assert.True(success);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var user2 = await repository.LoadUser(id);
//             Assert.Equal(id, user2.Id);
//             Assert.Equal(tenantId, user2.TenantId);
//             Assert.Equal(personName, user2.PersonName);
//             Assert.Equal(displayName, user2.DisplayName);
//             Assert.True(Equals(address, user2.Address));
//             Assert.True(Equals(phone,  user2.Phone));
//             Assert.True(Equals(email,user2.Email));
//             Assert.False(user2.Active);
//         }
//         
//         [Fact]
//         public async Task User_Activate()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var tenantId = Guid.NewGuid();
//             var personName = new PersonName("Joe", "Blow");
//             var displayName = "Joey Blowie";
//             var address = new Address("123 Main St.", "Suite 313", "Box 3", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//            
//             // Aggregate object function triggers an event on aggregate
//             var user = new User(id, tenantId,personName, displayName, address, phone, email);
//             
//             user.Deactivate();
//             user.Activate();
//             
//             var repository = GetRepository();
//             var success = await repository.SaveUser(userInfo, user);
//             Assert.True(success);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var user2 = await repository.LoadUser(id);
//             Assert.Equal(id, user2.Id);
//             Assert.Equal(tenantId, user2.TenantId);
//             Assert.Equal(personName, user2.PersonName);
//             Assert.Equal(displayName, user2.DisplayName);
//             Assert.True(Equals(address, user2.Address));
//             Assert.True(Equals(phone,  user2.Phone));
//             Assert.True(Equals(email,user2.Email));
//             Assert.True(user2.Active);
//         }
//         [Fact]
//         public async Task User_ChangeAuth0Id()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var tenantId = Guid.NewGuid();
//             var personName = new PersonName("Joe", "Blow");
//             var displayName = "Joey Blowie";
//             var address = new Address("123 Main St.", "Suite 313", "Box 3", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//            
//             // Aggregate object function triggers an event on aggregate
//             var user = new User(id, tenantId,personName, displayName, address, phone, email);
//             var auth0Id = "auth0|abcdefg123456";
//             user.ChangeAuth0Id(auth0Id);
//             
//             var repository = GetRepository();
//             var success = await repository.SaveUser(userInfo, user);
//             Assert.True(success);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var user2 = await repository.LoadUser(id);
//             Assert.Equal(id, user2.Id);
//             Assert.Equal(tenantId, user2.TenantId);
//             Assert.Equal(personName, user2.PersonName);
//             Assert.Equal(displayName, user2.DisplayName);
//             Assert.True(Equals(address, user2.Address));
//             Assert.True(Equals(phone,  user2.Phone));
//             Assert.True(Equals(email,user2.Email));
//             Assert.True(user2.Active);
//             Assert.Equal(auth0Id, user2.Auth0Id);
//         }
//         
//         private CrmRepository GetRepository()
//         { 
//             var eventTypeResolver = new EventTypeResolver();
//             
//             var eventStore = new InMemoryEventStore(
//                 eventTypeResolver,
//                 new Dictionary<string, List<string>>());
//             
//             var snapshotStore = new CosmosSnapshotStore(_testConfig.EndpointUri,
//                 _testConfig.AuthKey,
//                 _testConfig.Database, _testConfig.SnapshotContainer);
//             return new CrmRepository(eventStore, snapshotStore);
//         }
//       
//     }
// }