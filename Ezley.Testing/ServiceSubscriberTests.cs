// using System;
// using System.Threading;
// using System.Threading.Tasks;
// using Ezley.CQRS.Commands;
// using Ezley.Domain.CRM;
// using Ezley.Domain.CRM.Repositories;
// using Ezley.Events;
// using Ezley.EventSourcing;
// using Ezley.EventSourcing.Shared;
// using Ezley.Projections;
// using Ezley.ProjectionStore;
// using Ezley.SnapshotStore;
// using Ezley.ValueObjects;
// using Xunit;
//
// namespace Ezley.Testing
// {
//     public class ServiceSubscriberTests
//     {
//         private int delayMs = 3000;
//         private TestConfig _testConfig = new TestConfig();
//
//         [Fact]
//         public async Task RegisterServiceSubscriber()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var tenantId = Guid.NewGuid();
//             var legalName = "Rainy Day Fund, LLC";
//             var displayName = "Rainy Day Fun";
//             var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//
//             // Aggregate object function triggers an event on aggregate
//             var serviceSubscriber = new ServiceSubscriber(id,tenantId, legalName, displayName, address, phone, email);
//
//             var repository = GetCrmRepository();
//
//             // Save aggregate. Persists the events created.
//             var success = await repository.SaveServiceSubscriber(userInfo, serviceSubscriber);
//
//             Assert.True(success);
//             await Task.Delay(delayMs);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var serviceSubscriber2 = await repository.LoadServiceSubscriber(serviceSubscriber.Id);
//             Assert.Equal(id, serviceSubscriber2.Id);
//             Assert.Equal(tenantId, serviceSubscriber2.TenantId);
//             Assert.Equal(legalName, serviceSubscriber2.LegalName);
//             Assert.Equal(displayName, serviceSubscriber2.DisplayName);
//             Assert.True(Equals(address, serviceSubscriber2.Address));
//             Assert.True(Equals(phone,  serviceSubscriber2.Phone));
//             Assert.True(Equals(email,serviceSubscriber2.Email));
//             Assert.True(serviceSubscriber2.Active);
//         }
//         
//         [Fact]
//         public async Task RegisterServiceSubscriber_Projection()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var TenantId = Guid.NewGuid();
//             var legalName = "Rainy Day Fund, LLC";
//             var displayName = "Rainy Day Fun";
//             var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//
//             // Aggregate object function triggers an event on aggregate
//             var serviceSubscriber = new ServiceSubscriber(id, TenantId, legalName, displayName, address, phone, email);
//
//             var repository = GetCrmRepository();
//
//             // Save aggregate. Persists the events created.
//             var success = await repository.SaveServiceSubscriber(userInfo, serviceSubscriber);
//
//             Assert.True(success);
//             await Task.Delay(delayMs);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var viewRepo = this.GetViewRepository();
//             string templateName = $"ServiceSubscriberView:{id}";
//             var templateView = await viewRepo.LoadTypedViewAsync<ServiceSubscriberView>(templateName);
//             
//             Assert.Equal(id, templateView.Id);
//             Assert.Equal(TenantId, templateView.TenantId);
//             Assert.Equal(legalName, templateView.LegalName);
//             Assert.Equal(displayName, templateView.DisplayName);
//             Assert.True(Equals(address, templateView.Address));
//             Assert.True(Equals(phone,  templateView.Phone));
//             Assert.True(Equals(email,templateView.Email));
//             Assert.True(templateView.Active);
//         }
//
//
//         [Fact]
//         public async Task ServiceSubscriber_ChangeAddress()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var legalName = "Rainy Day Fund, LLC";
//             var displayName = "Rainy Day Fun";
//             var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//             var TenantId = Guid.NewGuid();
//             // Aggregate object function triggers an event on aggregate
//             var serviceSubscriber = new ServiceSubscriber(id, TenantId, legalName, displayName, address, phone, email);
//
//             var repository = GetCrmRepository();
//
//             // Save aggregate. Persists the events created.
//             var success = await repository.SaveServiceSubscriber(userInfo, serviceSubscriber);
//             Assert.True(success);
//
//             // Edit and save
//             var newAddress = new Address("newLine1", "newLine2", "newLine3", 
//                 "newCity", "27401","newState", "newCountry");
//             var sp2 = await repository.LoadServiceSubscriber(id);
//             sp2.ChangeAddress(newAddress);
//             await repository.SaveServiceSubscriber(userInfo, sp2);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var serviceSubscriber2 = await repository.LoadServiceSubscriber(serviceSubscriber.Id);
//             Assert.Equal(id, serviceSubscriber2.Id);
//             Assert.Equal(TenantId, serviceSubscriber2.TenantId);
//             Assert.Equal(legalName, serviceSubscriber2.LegalName);
//             Assert.Equal(displayName, serviceSubscriber2.DisplayName);
//             Assert.True(Equals(newAddress, serviceSubscriber2.Address));
//             Assert.True(Equals(phone,  serviceSubscriber2.Phone));
//             Assert.True(Equals(email,serviceSubscriber2.Email));
//             Assert.True(serviceSubscriber2.Active);
//         }
//         
//         [Fact]
//         public async Task ServiceSubscriber_ChangePhone()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var legalName = "Rainy Day Fund, LLC";
//             var displayName = "Rainy Day Fun";
//             var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//             var TenantId = Guid.NewGuid();
//             // Aggregate object function triggers an event on aggregate
//             var serviceSubscriber = new ServiceSubscriber(id, TenantId, legalName, displayName, address, phone, email);
//
//             var repository = GetCrmRepository();
//
//             // Save aggregate. Persists the events created.
//             var success = await repository.SaveServiceSubscriber(userInfo, serviceSubscriber);
//             Assert.True(success);
//
//             // Edit and save
//             var newPhone = new Phone("3361234567");
//             var sp2 = await repository.LoadServiceSubscriber(id);
//             sp2.ChangePhone(newPhone);
//             await repository.SaveServiceSubscriber(userInfo, sp2);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var serviceSubscriber2 = await repository.LoadServiceSubscriber(serviceSubscriber.Id);
//             Assert.Equal(id, serviceSubscriber2.Id);
//             Assert.Equal(TenantId, serviceSubscriber2.TenantId);
//             Assert.Equal(legalName, serviceSubscriber2.LegalName);
//             Assert.Equal(displayName, serviceSubscriber2.DisplayName);
//             Assert.True(Equals(address, serviceSubscriber2.Address));
//             Assert.True(Equals(newPhone,  serviceSubscriber2.Phone));
//             Assert.True(Equals(email,serviceSubscriber2.Email));
//             Assert.True(serviceSubscriber2.Active);
//         }
//         
//         [Fact]
//         public async Task ServiceSubscriber_ChangeEmail()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var legalName = "Rainy Day Fund, LLC";
//             var displayName = "Rainy Day Fun";
//             var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//             var TenantId = Guid.NewGuid();
//             // Aggregate object function triggers an event on aggregate
//             var serviceSubscriber = new ServiceSubscriber(id, TenantId, legalName, displayName, address, phone, email);
//
//             var repository = GetCrmRepository();
//
//             // Save aggregate. Persists the events created.
//             var success = await repository.SaveServiceSubscriber(userInfo, serviceSubscriber);
//             Assert.True(success);
//
//             // Edit and save
//             var newEmail = new Email("marc2@altavista.com");
//             var sp2 = await repository.LoadServiceSubscriber(id);
//             sp2.ChangeEmail(newEmail);
//             await repository.SaveServiceSubscriber(userInfo, sp2);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var serviceSubscriber2 = await repository.LoadServiceSubscriber(serviceSubscriber.Id);
//             Assert.Equal(id, serviceSubscriber2.Id);
//             Assert.Equal(TenantId, serviceSubscriber2.TenantId);
//             Assert.Equal(legalName, serviceSubscriber2.LegalName);
//             Assert.Equal(displayName, serviceSubscriber2.DisplayName);
//             Assert.True(Equals(address, serviceSubscriber2.Address));
//             Assert.True(Equals(phone,  serviceSubscriber2.Phone));
//             Assert.True(Equals(newEmail,serviceSubscriber2.Email));
//             Assert.True(serviceSubscriber2.Active);
//         }
//         
//         [Fact]
//         public async Task ServiceSubscriber_ChangeLegalName()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var legalName = "Rainy Day Fund, LLC";
//             var displayName = "Rainy Day Fun";
//             var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//             var TenantId = Guid.NewGuid();
//             
//             // Aggregate object function triggers an event on aggregate
//             var serviceSubscriber = new ServiceSubscriber(id, TenantId, legalName, displayName, address, phone, email);
//
//             var repository = GetCrmRepository();
//
//             // Save aggregate. Persists the events created.
//             var success = await repository.SaveServiceSubscriber(userInfo, serviceSubscriber);
//             Assert.True(success);
//
//             // Edit and save
//             var newLegalName = "legalName2";
//             var sp2 = await repository.LoadServiceSubscriber(id);
//             sp2.ChangeLegalName(newLegalName);
//             await repository.SaveServiceSubscriber(userInfo, sp2);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var serviceSubscriber2 = await repository.LoadServiceSubscriber(serviceSubscriber.Id);
//             Assert.Equal(id, serviceSubscriber2.Id);
//             Assert.Equal(TenantId, serviceSubscriber2.TenantId);
//             Assert.Equal(newLegalName, serviceSubscriber2.LegalName);
//             Assert.Equal(displayName, serviceSubscriber2.DisplayName);
//             Assert.True(Equals(address, serviceSubscriber2.Address));
//             Assert.True(Equals(phone,  serviceSubscriber2.Phone));
//             Assert.True(Equals(email,serviceSubscriber2.Email));
//             Assert.True(serviceSubscriber2.Active);
//         }
//         
//         [Fact]
//         public async Task ServiceSubscriber_ChangeDisplayName()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var legalName = "Rainy Day Fund, LLC";
//             var displayName = "Rainy Day Fun";
//             var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//             var TenantId = Guid.NewGuid();
//             
//             // Aggregate object function triggers an event on aggregate
//             var serviceSubscriber = new ServiceSubscriber(id, TenantId, legalName, displayName, address, phone, email);
//
//             var repository = GetCrmRepository();
//
//             // Save aggregate. Persists the events created.
//             var success = await repository.SaveServiceSubscriber(userInfo, serviceSubscriber);
//             Assert.True(success);
//
//             // Edit and save
//             var newDisplayName = "newDisplayName";
//             var sp2 = await repository.LoadServiceSubscriber(id);
//             sp2.ChangeDisplayName(newDisplayName);
//             await repository.SaveServiceSubscriber(userInfo, sp2);
//             
//             // now some time goes by so we pull it from Ezley.EventStore
//             var serviceSubscriber2 = await repository.LoadServiceSubscriber(serviceSubscriber.Id);
//             Assert.Equal(id, serviceSubscriber2.Id);
//             Assert.Equal(TenantId, serviceSubscriber2.TenantId);
//             Assert.Equal(legalName, serviceSubscriber2.LegalName);
//             Assert.Equal(newDisplayName, serviceSubscriber2.DisplayName);
//             Assert.True(Equals(address, serviceSubscriber2.Address));
//             Assert.True(Equals(phone,  serviceSubscriber2.Phone));
//             Assert.True(Equals(email,serviceSubscriber2.Email));
//             Assert.True(serviceSubscriber2.Active);
//         }
//         
//        
//         [Fact]
//         public async Task ServiceSubscriber_EditCommand()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var legalName = "Rainy Day Fund, LLC";
//             var displayName = "Rainy Day Fun";
//             var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//             var active = true;
//             var TenantId = Guid.NewGuid();
//             
//             // Aggregate object function triggers an event on aggregate
//             var serviceSubscriber = new ServiceSubscriber(id, TenantId, legalName, displayName, address, phone, email);
//             var repository = GetCrmRepository();
//             
//             // Save aggregate. Persists the events created.
//             var success = await repository.SaveServiceSubscriber(userInfo, serviceSubscriber);
//             
//             var legalName2 = "2Rainy Day Fund, LLC";
//             var displayName2 = "2Rainy Day Fun";
//             var address2 = new Address("2123 Main St.", "2Suite 313", "2", "2Greensboro", "27401","NC", "US");
//             var email2 = new Email("2Marc@gmail.com");
//             var phone2 = new Phone("23365551234");
//
//             var command = new EditServiceSubscriber(
//                 userInfo,
//                 id,
//                 new Editable<string>(legalName2),
//                 new Editable<string>(displayName2),
//                 new Editable<Address>(address2),
//                 new Editable<Phone>(phone2),
//                 new Editable<Email>(email2),
//                 new Editable<bool>(active));
//             
//             // Issue command
//             var handler = new EditServiceSubscriberHandler(repository);
//             var token = new CancellationToken();
//             await handler.Handle(command, token);
//             
//             // Check data
//             // now some time goes by so we pull it from Ezley.EventStore
//             var serviceSubscriber2 = await repository.LoadServiceSubscriber(id);
//             Assert.Equal(id, serviceSubscriber2.Id);
//             Assert.Equal(TenantId,serviceSubscriber2.TenantId);
//             Assert.Equal(legalName2, serviceSubscriber2.LegalName);
//             Assert.Equal(displayName2, serviceSubscriber2.DisplayName);
//             Assert.True(Equals(address2, serviceSubscriber2.Address));
//             Assert.True(Equals(phone2,  serviceSubscriber2.Phone));
//             Assert.True(Equals(email2,serviceSubscriber2.Email));
//             Assert.True(serviceSubscriber2.Active);
//         }
//         
//         
//          [Fact]
//         public async Task ServiceSubscriber_Deactivate()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var legalName = "Rainy Day Fund, LLC";
//             var displayName = "Rainy Day Fun";
//             var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//             var TenantId = Guid.NewGuid();
//             
//             // Aggregate object function triggers an event on aggregate
//             var serviceSubscriber = new ServiceSubscriber(id, TenantId, legalName, displayName, address, phone, email);
//             var repository = GetCrmRepository();
//             
//             // Save aggregate. Persists the events created.
//             var success = await repository.SaveServiceSubscriber(userInfo, serviceSubscriber);
//
//             var serviceSubscriber2 = await repository.LoadServiceSubscriber(id);
//             serviceSubscriber2.Deactivate();
//
//             Assert.True(await repository.SaveServiceSubscriber(userInfo, serviceSubscriber2,null));
//               
//             // Check data
//             // now some time goes by so we pull it from Ezley.EventStore
//             var serviceSubscriber3 = await repository.LoadServiceSubscriber(id);
//             Assert.Equal(id, serviceSubscriber3.Id);
//             Assert.Equal(TenantId, serviceSubscriber3.TenantId);
//             Assert.Equal(legalName, serviceSubscriber3.LegalName);
//             Assert.Equal(displayName, serviceSubscriber3.DisplayName);
//             Assert.True(Equals(address, serviceSubscriber3.Address));
//             Assert.True(Equals(phone,  serviceSubscriber3.Phone));
//             Assert.True(Equals(email,serviceSubscriber3.Email));
//             Assert.False(serviceSubscriber3.Active);
//         }
//         
//         [Fact]
//         public async Task ServiceSubscriber_Activate()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var legalName = "Rainy Day Fund, LLC";
//             var displayName = "Rainy Day Fun";
//             var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//             var TenantId = Guid.NewGuid();
//             
//             // Aggregate object function triggers an event on aggregate
//             var serviceSubscriber = new ServiceSubscriber(id, TenantId, legalName, displayName, address, phone, email);
//             var repository = GetCrmRepository();
//             
//             // Save aggregate. Persists the events created.
//             var success = await repository.SaveServiceSubscriber(userInfo, serviceSubscriber);
//
//             var serviceSubscriber2 = await repository.LoadServiceSubscriber(id);
//             serviceSubscriber2.Deactivate();
//
//             Assert.True(await repository.SaveServiceSubscriber(userInfo, serviceSubscriber2,null));
//               
//             var serviceSubscriber3 = await repository.LoadServiceSubscriber(id);
//             serviceSubscriber3.Activate();
//
//             Assert.True(await repository.SaveServiceSubscriber(userInfo, serviceSubscriber3,null));
//
//             // Check data
//             // now some time goes by so we pull it from Ezley.EventStore
//             var serviceSubscriber4 = await repository.LoadServiceSubscriber(id);
//             Assert.Equal(id, serviceSubscriber4.Id);
//             Assert.Equal(TenantId, serviceSubscriber4.TenantId);
//             Assert.Equal(legalName, serviceSubscriber4.LegalName);
//             Assert.Equal(displayName, serviceSubscriber4.DisplayName);
//             Assert.True(Equals(address, serviceSubscriber4.Address));
//             Assert.True(Equals(phone,  serviceSubscriber4.Phone));
//             Assert.True(Equals(email,serviceSubscriber4.Email));
//             Assert.True(serviceSubscriber4.Active);
//         }
//         [Fact]
//         public async Task ServiceSubscriber_EditCommand_SnapshotTest()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var legalName = "Rainy Day Fund, LLC";
//             var displayName = "Rainy Day Fun";
//             var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//             var active = true;
//             var TenantId = Guid.NewGuid();
//             
//             // Aggregate object function triggers an event on aggregate
//             var serviceSubscriber = new ServiceSubscriber(id, TenantId, legalName, displayName, address, phone, email);
//             var repository = GetCrmRepository();
//
//             var snapshot1 = serviceSubscriber.GetSnapshot();
//             // Save aggregate. Persists the events created.
//             var success = await repository.SaveServiceSubscriber(userInfo, serviceSubscriber, snapshot1);
//             
//             
//             var legalName2 = "2Rainy Day Fund, LLC";
//             var displayName2 = "2Rainy Day Fun";
//             var address2 = new Address("2123 Main St.", "2Suite 313", "2", "2Greensboro", "27401","NC", "US");
//             var email2 = new Email("2Marc@gmail.com");
//             var phone2 = new Phone("23365551234");
//
//             var command = new EditServiceSubscriber(
//                 userInfo,
//                 id,
//                 new Editable<string>(legalName2),
//                 new Editable<string>(displayName2),
//                 new Editable<Address>(address2),
//                 new Editable<Phone>(phone2),
//                 new Editable<Email>(email2),
//                 new Editable<bool>(active));
//             
//             // Issue command
//             var handler = new EditServiceSubscriberHandler(repository);
//             var token = new CancellationToken();
//             await handler.Handle(command, token);
//             
//             // Check data
//             // now some time goes by so we pull it from Ezley.EventStore
//             var serviceSubscriber2 = await repository.LoadServiceSubscriber(id);
//
//
//             Assert.Equal(id, serviceSubscriber2.Id);
//             Assert.Equal(TenantId, serviceSubscriber2.TenantId);
//             Assert.Equal(legalName2, serviceSubscriber2.LegalName);
//             Assert.Equal(displayName2, serviceSubscriber2.DisplayName);
//             Assert.True(Equals(address2, serviceSubscriber2.Address));
//             Assert.True(Equals(phone2,  serviceSubscriber2.Phone));
//             Assert.True(Equals(email2,serviceSubscriber2.Email));
//             Assert.True(serviceSubscriber2.Active);
//
//             var legalName3 = "Another legal name.";
//             var displayName3 = "Another dislayName";
//             serviceSubscriber2.ChangeLegalName(legalName3);
//             var snapshot2 = serviceSubscriber2.GetSnapshot();
//             
//             serviceSubscriber2.ChangeDisplayName(displayName3);
//             Assert.True(await repository.SaveServiceSubscriber(userInfo, serviceSubscriber2, snapshot2));
//              
//             var serviceSubscriber3 = await repository.LoadServiceSubscriber(id);
//             Assert.Equal(id, serviceSubscriber3.Id);
//             Assert.Equal(legalName3, serviceSubscriber3.LegalName);
//             Assert.Equal(displayName3, serviceSubscriber3.DisplayName);
//             Assert.True(Equals(address2, serviceSubscriber3.Address));
//             Assert.True(Equals(phone2,  serviceSubscriber3.Phone));
//             Assert.True(Equals(email2,serviceSubscriber3.Email));
//             Assert.True(serviceSubscriber3.Active);
//
//         }
//         
//         
//         [Fact]
//         public async Task ServiceSubscriber_EditCommand_LegalNameOnly()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var legalName = "Rainy Day Fund, LLC";
//             var displayName = "Rainy Day Fun";
//             var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//             var active = true;
//             var TenantId = Guid.NewGuid();
//             
//             // Aggregate object function triggers an event on aggregate
//             var serviceSubscriber = new ServiceSubscriber(id, TenantId, legalName, displayName, address, phone, email);
//             var repository = GetCrmRepository();
//             
//             // Save aggregate. Persists the events created.
//             var success = await repository.SaveServiceSubscriber(userInfo, serviceSubscriber);
//             
//             var legalName2 = "2Rainy Day Fund, LLC";
//           
//             var command = new EditServiceSubscriber(
//                 userInfo,
//                 id,
//                 new Editable<string>(legalName2),
//                 new Editable<string>(),
//                 new Editable<Address>(),
//                 new Editable<Phone>(),
//                 new Editable<Email>(),
//                 new Editable<bool>(active));
//             
//             // Issue command
//             var handler = new EditServiceSubscriberHandler(repository);
//             var token = new CancellationToken();
//             await handler.Handle(command, token);
//             
//             // Check data
//             // now some time goes by so we pull it from Ezley.EventStore
//             var serviceSubscriber2 = await repository.LoadServiceSubscriber(id);
//             Assert.Equal(id, serviceSubscriber2.Id);
//             Assert.Equal(TenantId, serviceSubscriber2.TenantId);
//             Assert.Equal(legalName2, serviceSubscriber2.LegalName);
//             Assert.Equal(displayName, serviceSubscriber2.DisplayName);
//             Assert.True(Equals(address, serviceSubscriber2.Address));
//             Assert.True(Equals(phone,  serviceSubscriber2.Phone));
//             Assert.True(Equals(email,serviceSubscriber2.Email));
//             Assert.True(serviceSubscriber2.Active);
//         }
//         
//         [Fact]
//         public async Task ServiceSubscriber_EditCommand_AddressOnly()
//         {
//              var userId = "test|abcd123xx456efg";
//             var userInfo = new EventUserInfo(userId);
//             var id = Guid.NewGuid();
//             var legalName = "Rainy Day Fund, LLC";
//             var displayName = "Rainy Day Fun";
//             var address = new Address("123 Main St.", "Suite 313", "", "Greensboro", "27401","NC", "US");
//             var email = new Email("Marc@gmail.com");
//             var phone = new Phone("3365551234");
//             var active = true;
//             var TenantId = Guid.NewGuid();
//             
//             // Aggregate object function triggers an event on aggregate
//             var serviceSubscriber = new ServiceSubscriber(id, TenantId, legalName, displayName, address, phone, email);
//             var repository = GetCrmRepository();
//             
//             // Save aggregate. Persists the events created.
//             var success = await repository.SaveServiceSubscriber(userInfo, serviceSubscriber);
//
//              var address2 = new Address("2123 Main St.", "2Suite 313", "2", "2Greensboro", "27401","NC", "US");
//
//              var command = new EditServiceSubscriber(
//                  userInfo,
//                 id,
//                 new Editable<string>(),
//                 new Editable<string>(),
//                 new Editable<Address>(address2),
//                 new Editable<Phone>(),
//                 new Editable<Email>(),
//                 new Editable<bool>(active));
//             
//             // Issue command
//             var handler = new EditServiceSubscriberHandler(repository);
//             var token = new CancellationToken();
//             await handler.Handle(command, token);
//             
//             // Check data
//             // now some time goes by so we pull it from Ezley.EventStore
//             var serviceSubscriber2 = await repository.LoadServiceSubscriber(id);
//             Assert.Equal(id, serviceSubscriber2.Id);
//             Assert.Equal(TenantId, serviceSubscriber2.TenantId);
//             Assert.Equal(legalName, serviceSubscriber2.LegalName);
//             Assert.Equal(displayName, serviceSubscriber2.DisplayName);
//             Assert.True(Equals(address2, serviceSubscriber2.Address));
//             Assert.True(Equals(phone,  serviceSubscriber2.Phone));
//             Assert.True(Equals(email,serviceSubscriber2.Email));
//             Assert.True(serviceSubscriber2.Active);
//         }
//        
//         private CrmRepository GetCrmRepository()
//         { 
//             var eventTypeResolver = new EventTypeResolver();
//             var eventStore = new CosmosDBEventStore(
//                 eventTypeResolver,
//                 _testConfig.EndpointUri, 
//                 _testConfig.AuthKey, _testConfig.Database, _testConfig.EventContainer);
//         
//             var snapshotStore = new CosmosSnapshotStore(_testConfig.EndpointUri,
//                 _testConfig.AuthKey,
//                 _testConfig.Database, _testConfig.SnapshotContainer);
//             return new CrmRepository(eventStore, snapshotStore);
//         }
//         
//         private CosmosDBViewRepository GetViewRepository()
//         {
//             return new CosmosDBViewRepository(_testConfig.EndpointUri, _testConfig.AuthKey,
//                 _testConfig.Database, _testConfig.ViewContainer);
//         }
//     }
// }