using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ezley.CQRS.Commands;
using Ezley.Domain.CRM;
using Ezley.Domain.CRM.Repositories;
using Ezley.Events;
using Ezley.EventSourcing;
using Ezley.EventStore;
using Ezley.Projections;
using Ezley.ProjectionStore;
using Ezley.SnapshotStore;
using Ezley.ValueObjects;
using Xunit;

namespace Ezley.Testing
{
    public class TenantTests
    {
        private int delayMs = 3000;
        private TestConfig _testConfig = new TestConfig();

        [Fact]
        public async Task RegisterTenant()
        {
             var userId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(userId);
            var id = Guid.NewGuid();
            var legalName = "Rainy Day Fund, LLC";
            var displayName = "Rainy Day Fun";
            var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
            var email = new Email("Marc@gmail.com");
            var phone = new Phone("3365551234");
           
            // Aggregate object function triggers an event on aggregate
            var Tenant = new Tenant(id, legalName, displayName, address, phone, email);

            var repository = GetCrmRepository();

            // Save aggregate. Persists the events created.
            var success = await repository.SaveTenant(userInfo, Tenant);

            Assert.True(success);
            await Task.Delay(delayMs);
            
            // now some time goes by so we pull it from Ezley.EventStore
            var Tenant2 = await repository.LoadTenant(Tenant.Id);
            Assert.Equal(id, Tenant2.Id);
            Assert.Equal(legalName, Tenant2.LegalName);
            Assert.Equal(displayName, Tenant2.DisplayName);
            Assert.True(Equals(address, Tenant2.Address));
            Assert.True(Equals(phone,  Tenant2.Phone));
            Assert.True(Equals(email,Tenant2.Email));
        }
        
        [Fact]
        public async Task RegisterTenant_Projection()
        {
             var userId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(userId);
            var id = Guid.NewGuid();
            var legalName = "Rainy Day Fund, LLC";
            var displayName = "Rainy Day Fun";
            var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
            var email = new Email("Marc@gmail.com");
            var phone = new Phone("3365551234");

            // Aggregate object function triggers an event on aggregate
            var Tenant = new Tenant(id, legalName, displayName, address, phone, email);

            var repository = GetCrmRepository();

            // Save aggregate. Persists the events created.
            var success = await repository.SaveTenant(userInfo, Tenant);

            Assert.True(success);
            await Task.Delay(delayMs);
            
            // now some time goes by so we pull it from Ezley.EventStore
            var viewRepo = this.GetViewRepository();
            string templateName = $"TenantView:{id}";
            var templateView = await viewRepo.LoadTypedViewAsync<TenantView>(templateName);
            
            Assert.Equal(id, templateView.Id);
            Assert.Equal(legalName, templateView.LegalName);
            Assert.Equal(displayName, templateView.DisplayName);
            Assert.True(Equals(address, templateView.Address));
            Assert.True(Equals(phone,  templateView.Phone));
            Assert.True(Equals(email,templateView.Email));
        }


        [Fact]
        public async Task Tenant_ChangeAddress()
        {
             var userId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(userId);
            var id = Guid.NewGuid();
            var legalName = "Rainy Day Fund, LLC";
            var displayName = "Rainy Day Fun";
            var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
            var email = new Email("Marc@gmail.com");
            var phone = new Phone("3365551234");

            // Aggregate object function triggers an event on aggregate
            var Tenant = new Tenant(id, legalName, displayName, address, phone, email);

            var repository = GetCrmRepository();

            // Save aggregate. Persists the events created.
            var success = await repository.SaveTenant(userInfo, Tenant);
            Assert.True(success);

            // Edit and save
            var newAddress = new Address("newLine1", "newLine2", "newLine3", 
                "newCity", "27401","newState", "newCountry");
            var sp2 = await repository.LoadTenant(id);
            sp2.ChangeAddress(newAddress);
            await repository.SaveTenant(userInfo, sp2);
            
            // now some time goes by so we pull it from Ezley.EventStore
            var Tenant2 = await repository.LoadTenant(Tenant.Id);
            Assert.Equal(id, Tenant2.Id);
            Assert.Equal(legalName, Tenant2.LegalName);
            Assert.Equal(displayName, Tenant2.DisplayName);
            Assert.True(Equals(newAddress, Tenant2.Address));
            Assert.True(Equals(phone,  Tenant2.Phone));
            Assert.True(Equals(email,Tenant2.Email));
        }
        
        [Fact]
        public async Task Tenant_ChangePhone()
        {
             var userId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(userId);
            var id = Guid.NewGuid();
            var legalName = "Rainy Day Fund, LLC";
            var displayName = "Rainy Day Fun";
            var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
            var email = new Email("Marc@gmail.com");
            var phone = new Phone("3365551234");

            // Aggregate object function triggers an event on aggregate
            var Tenant = new Tenant(id, legalName, displayName, address, phone, email);

            var repository = GetCrmRepository();

            // Save aggregate. Persists the events created.
            var success = await repository.SaveTenant(userInfo, Tenant);
            Assert.True(success);

            // Edit and save
            var newPhone = new Phone("3361234567");
            var sp2 = await repository.LoadTenant(id);
            sp2.ChangePhone(newPhone);
            await repository.SaveTenant(userInfo, sp2);
            
            // now some time goes by so we pull it from Ezley.EventStore
            var Tenant2 = await repository.LoadTenant(Tenant.Id);
            Assert.Equal(id, Tenant2.Id);
            Assert.Equal(legalName, Tenant2.LegalName);
            Assert.Equal(displayName, Tenant2.DisplayName);
            Assert.True(Equals(address, Tenant2.Address));
            Assert.True(Equals(newPhone,  Tenant2.Phone));
            Assert.True(Equals(email,Tenant2.Email));
        }
        
        [Fact]
        public async Task Tenant_ChangeEmail()
        {
             var userId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(userId);
            var id = Guid.NewGuid();
            var legalName = "Rainy Day Fund, LLC";
            var displayName = "Rainy Day Fun";
            var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
            var email = new Email("Marc@gmail.com");
            var phone = new Phone("3365551234");

            // Aggregate object function triggers an event on aggregate
            var Tenant = new Tenant(id, legalName, displayName, address, phone, email);

            var repository = GetCrmRepository();

            // Save aggregate. Persists the events created.
            var success = await repository.SaveTenant(userInfo, Tenant);
            Assert.True(success);

            // Edit and save
            var newEmail = new Email("marc2@altavista.com");
            var sp2 = await repository.LoadTenant(id);
            sp2.ChangeEmail(newEmail);
            await repository.SaveTenant(userInfo, sp2);
            
            // now some time goes by so we pull it from Ezley.EventStore
            var Tenant2 = await repository.LoadTenant(Tenant.Id);
            Assert.Equal(id, Tenant2.Id);
            Assert.Equal(legalName, Tenant2.LegalName);
            Assert.Equal(displayName, Tenant2.DisplayName);
            Assert.True(Equals(address, Tenant2.Address));
            Assert.True(Equals(phone,  Tenant2.Phone));
            Assert.True(Equals(newEmail,Tenant2.Email));
        }
        
        [Fact]
        public async Task Tenant_ChangeLegalName()
        {
             var userId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(userId);
            var id = Guid.NewGuid();
            var legalName = "Rainy Day Fund, LLC";
            var displayName = "Rainy Day Fun";
            var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
            var email = new Email("Marc@gmail.com");
            var phone = new Phone("3365551234");

            // Aggregate object function triggers an event on aggregate
            var Tenant = new Tenant(id, legalName, displayName, address, phone, email);

            var repository = GetCrmRepository();

            // Save aggregate. Persists the events created.
            var success = await repository.SaveTenant(userInfo, Tenant);
            Assert.True(success);

            // Edit and save
            var newLegalName = "legalName2";
            var sp2 = await repository.LoadTenant(id);
            sp2.ChangeLegalName(newLegalName);
            await repository.SaveTenant(userInfo, sp2);
            
            // now some time goes by so we pull it from Ezley.EventStore
            var Tenant2 = await repository.LoadTenant(Tenant.Id);
            Assert.Equal(id, Tenant2.Id);
            Assert.Equal(newLegalName, Tenant2.LegalName);
            Assert.Equal(displayName, Tenant2.DisplayName);
            Assert.True(Equals(address, Tenant2.Address));
            Assert.True(Equals(phone,  Tenant2.Phone));
            Assert.True(Equals(email,Tenant2.Email));
        }
        
        [Fact]
        public async Task Tenant_ChangeDisplayName()
        {
             var userId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(userId);
            var id = Guid.NewGuid();
            var legalName = "Rainy Day Fund, LLC";
            var displayName = "Rainy Day Fun";
            var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
            var email = new Email("Marc@gmail.com");
            var phone = new Phone("3365551234");

            // Aggregate object function triggers an event on aggregate
            var Tenant = new Tenant(id, legalName, displayName, address, phone, email);

            var repository = GetCrmRepository();

            // Save aggregate. Persists the events created.
            var success = await repository.SaveTenant(userInfo, Tenant);
            Assert.True(success);

            // Edit and save
            var newDisplayName = "newDisplayName";
            var sp2 = await repository.LoadTenant(id);
            sp2.ChangeDisplayName(newDisplayName);
            await repository.SaveTenant(userInfo, sp2);
            
            // now some time goes by so we pull it from Ezley.EventStore
            var Tenant2 = await repository.LoadTenant(Tenant.Id);
            Assert.Equal(id, Tenant2.Id);
            Assert.Equal(legalName, Tenant2.LegalName);
            Assert.Equal(newDisplayName, Tenant2.DisplayName);
            Assert.True(Equals(address, Tenant2.Address));
            Assert.True(Equals(phone,  Tenant2.Phone));
            Assert.True(Equals(email,Tenant2.Email));
        }
        
       
        [Fact]
        public async Task Tenant_EditCommand()
        {
             var userId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(userId);
            var id = Guid.NewGuid();
            var legalName = "Rainy Day Fund, LLC";
            var displayName = "Rainy Day Fun";
            var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
            var email = new Email("Marc@gmail.com");
            var phone = new Phone("3365551234");
            // Aggregate object function triggers an event on aggregate
            var Tenant = new Tenant(id, legalName, displayName, address, phone, email);
            var repository = GetCrmRepository();
            
            // Save aggregate. Persists the events created.
            var success = await repository.SaveTenant(userInfo, Tenant);
            
            var legalName2 = "2Rainy Day Fund, LLC";
            var displayName2 = "2Rainy Day Fun";
            var address2 = new Address("2123 Main St.", "2Suite 313", "2", "2Greensboro", "27401","NC", "US");
            var email2 = new Email("2Marc@gmail.com");
            var phone2 = new Phone("23365551234");

            var command = new EditTenant(
                userInfo,
                id,
                new Editable<string>(legalName2),
                new Editable<string>(displayName2),
                new Editable<Address>(address2),
                new Editable<Phone>(phone2),
                new Editable<Email>(email2));
            
            // Issue command
            var handler = new EditTenantHandler(repository);
            var token = new CancellationToken();
            await handler.Handle(command, token);
            
            // Check data
            // now some time goes by so we pull it from Ezley.EventStore
            var Tenant2 = await repository.LoadTenant(id);
            Assert.Equal(id, Tenant2.Id);
            Assert.Equal(legalName2, Tenant2.LegalName);
            Assert.Equal(displayName2, Tenant2.DisplayName);
            Assert.True(Equals(address2, Tenant2.Address));
            Assert.True(Equals(phone2,  Tenant2.Phone));
            Assert.True(Equals(email2,Tenant2.Email));
        }
        
        
        [Fact]
        public async Task Tenant_EditCommand_SnapshotTest()
        {
             var userId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(userId);
            var id = Guid.NewGuid();
            var legalName = "Rainy Day Fund, LLC";
            var displayName = "Rainy Day Fun";
            var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
            var email = new Email("Marc@gmail.com");
            var phone = new Phone("3365551234");
            // Aggregate object function triggers an event on aggregate
            var Tenant = new Tenant(id, legalName, displayName, address, phone, email);
            var repository = GetCrmRepository();

            var snapshot1 = Tenant.GetSnapshot();
            // Save aggregate. Persists the events created.
            var success = await repository.SaveTenant(userInfo, Tenant, snapshot1);
            
            var legalName2 = "2Rainy Day Fund, LLC";
            var displayName2 = "2Rainy Day Fun";
            var address2 = new Address("2123 Main St.", "2Suite 313", "2", "2Greensboro", "27401","NC", "US");
            var email2 = new Email("2Marc@gmail.com");
            var phone2 = new Phone("23365551234");

            var command = new EditTenant(
                userInfo,
                id,
                new Editable<string>(legalName2),
                new Editable<string>(displayName2),
                new Editable<Address>(address2),
                new Editable<Phone>(phone2),
                new Editable<Email>(email2));
            
            // Issue command
            var handler = new EditTenantHandler(repository);
            var token = new CancellationToken();
            await handler.Handle(command, token);
            
            // Check data
            // now some time goes by so we pull it from Ezley.EventStore
            var Tenant2 = await repository.LoadTenant(id);
            
            Assert.Equal(id, Tenant2.Id);
            Assert.Equal(legalName2, Tenant2.LegalName);
            Assert.Equal(displayName2, Tenant2.DisplayName);
            Assert.True(Equals(address2, Tenant2.Address));
            Assert.True(Equals(phone2,  Tenant2.Phone));
            Assert.True(Equals(email2,Tenant2.Email));

            var legalName3 = "Another legal name.";
            var displayName3 = "Another dislayName";
            Tenant2.ChangeLegalName(legalName3);
            var snapshot2 = Tenant2.GetSnapshot();
            
            Tenant2.ChangeDisplayName(displayName3);
            Assert.True(await repository.SaveTenant(userInfo, Tenant2, snapshot2));
             
            var Tenant3 = await repository.LoadTenant(id);
            Assert.Equal(id, Tenant3.Id);
            Assert.Equal(legalName3, Tenant3.LegalName);
            Assert.Equal(displayName3, Tenant3.DisplayName);
            Assert.True(Equals(address2, Tenant3.Address));
            Assert.True(Equals(phone2,  Tenant3.Phone));
            Assert.True(Equals(email2,Tenant3.Email));

        }
        
        
        [Fact]
        public async Task Tenant_EditCommand_LegalNameOnly()
        {
             var userId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(userId);
            var id = Guid.NewGuid();
            var legalName = "Rainy Day Fund, LLC";
            var displayName = "Rainy Day Fun";
            var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
            var email = new Email("Marc@gmail.com");
            var phone = new Phone("3365551234");
            // Aggregate object function triggers an event on aggregate
            var Tenant = new Tenant(id, legalName, displayName, address, phone, email);
            var repository = GetCrmRepository();
            
            // Save aggregate. Persists the events created.
            var success = await repository.SaveTenant(userInfo, Tenant);
            
            var legalName2 = "2Rainy Day Fund, LLC";

            var command = new EditTenant(
                userInfo,
                id,
                new Editable<string>(legalName2),
                new Editable<string>(),
                new Editable<Address>(),
                new Editable<Phone>(),
                new Editable<Email>());
            
            // Issue command
            var handler = new EditTenantHandler(repository);
            var token = new CancellationToken();
            await handler.Handle(command, token);
            
            // Check data
            // now some time goes by so we pull it from Ezley.EventStore
            var Tenant2 = await repository.LoadTenant(id);
            Assert.Equal(id, Tenant2.Id);
            Assert.Equal(legalName2, Tenant2.LegalName);
            Assert.Equal(displayName, Tenant2.DisplayName);
            Assert.True(Equals(address, Tenant2.Address));
            Assert.True(Equals(phone,  Tenant2.Phone));
            Assert.True(Equals(email,Tenant2.Email));
        }
        
         [Fact]
        public async Task Tenant_EditCommand_AddressOnly()
        {
            var userId = "test|abcd123xx456efg";
            var userInfo = new EventUserInfo(userId);
            var id = Guid.NewGuid();
            var legalName = "Rainy Day Fund, LLC";
            var displayName = "Rainy Day Fun";
            var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
            var email = new Email("Marc@gmail.com");
            var phone = new Phone("3365551234");
            // Aggregate object function triggers an event on aggregate
            var Tenant = new Tenant(id, legalName, displayName, address, phone, email);
            var repository = GetCrmRepository();
            
            // Save aggregate. Persists the events created.
            var success = await repository.SaveTenant(userInfo, Tenant);
            
            var address2 = new Address("2123 Main St.", "2Suite 313", "2", "2Greensboro", "27401","NC", "US");

            var command = new EditTenant(
                userInfo,
                id,
                new Editable<string>(),
                new Editable<string>(),
                new Editable<Address>(address2),
                new Editable<Phone>(),
                new Editable<Email>());
            
            // Issue command
            var handler = new EditTenantHandler(repository);
            var token = new CancellationToken();
            await handler.Handle(command, token);
            
            // Check data
            // now some time goes by so we pull it from Ezley.EventStore
            var Tenant2 = await repository.LoadTenant(id);
            Assert.Equal(id, Tenant2.Id);
            Assert.Equal(legalName, Tenant2.LegalName);
            Assert.Equal(displayName, Tenant2.DisplayName);
            Assert.True(Equals(address2, Tenant2.Address));
            Assert.True(Equals(phone,  Tenant2.Phone));
            Assert.True(Equals(email,Tenant2.Email));
        }
        
        private CrmRepository GetCrmRepository()
        { 
            var eventTypeResolver = new EventTypeResolver();
            // var eventStore = new CosmosDBEventStore(
            //     eventTypeResolver,
            //     _testConfig.EndpointUri, 
            //     _testConfig.AuthKey, _testConfig.Database, _testConfig.EventContainer);
            var eventStore = new InMemoryEventStore(
                 eventTypeResolver,
                 new Dictionary<string, List<string>>());
            var snapshotStore = new CosmosSnapshotStore(_testConfig.EndpointUri,
                _testConfig.AuthKey,
                _testConfig.Database, _testConfig.SnapshotContainer);
            return new CrmRepository(eventStore, snapshotStore);
        }
        
        private CosmosDBViewRepository GetViewRepository()
        {
            return new CosmosDBViewRepository(_testConfig.EndpointUri, _testConfig.AuthKey,
                _testConfig.Database, _testConfig.ViewContainer);
        }
    }
}