using System;
using System.Collections.Generic;
using ES.Domain;
using Ezley.Events;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Domain.CRM
{
    public class ServiceSubscriber: AggregateBase
    {
        public Guid Id { get; private set; }
        public Guid TenantId { get; private set; }
        public string LegalName { get; private set; }
        public string DisplayName { get; private set; }
        public Address Address { get; private set; }
        public Phone Phone { get; private set; }
        public Email Email { get; private set; }
        public bool Active { get; private set; } = true;

        public ServiceSubscriber(IEnumerable<IEvent> events): base(events)
        {
        }
        public ServiceSubscriber(Guid id, Guid TenantId, string legalName, string displayName,
            Address address, Phone phone, Email email)
        {
            bool initialActiveValue = true;
            Apply(new ServiceSubscriberRegistered(id, TenantId, legalName,displayName,address,phone,email, initialActiveValue));
        }

        public void ChangeLegalName(string legalName)
        {
            if(this.LegalName != legalName)
                this.Apply(new ServiceSubscriberLegalNameChanged( this.Id,  this.TenantId, legalName));
        }
        public void ChangeDisplayName(string displayName)
        {
            if(this.DisplayName != displayName)
                this.Apply(new ServiceSubscriberDisplayNameChanged( this.Id, TenantId, displayName));
        }
        public void ChangeAddress(Address address)
        {
            if(!Equals(this.Address, address))
                this.Apply(new ServiceSubscriberAddressChanged( this.Id, address));
        }
        public void ChangePhone( Phone phone)
        {
            if(!Equals(Phone, phone))
                this.Apply(new ServiceSubscriberPhoneChanged( this.Id, phone));
        }
        public void ChangeEmail( Email email)
        {
            if(!Equals(Email, email))
                this.Apply(new ServiceSubscriberEmailChanged(this.Id, email));
        }
        public void Activate()
        {
            if(this.Active != true)
                this.Apply(new ServiceSubscriberActivated( this.Id, this.TenantId, this.LegalName, this.DisplayName));
        } public void Deactivate()
        {
            if(this.Active)
                this.Apply(new ServiceSubscriberDeactivated( this.Id, this.TenantId,this.LegalName, this.DisplayName));
        }
       
        
        #region WhenEvent_ApplyEventsOnly
        protected override void Mutate(IEvent @event)
        {
            ((dynamic) this).When((dynamic) @event);
        }
        protected void When(ServiceSubscriberRegistered command)
        {
            Id = command.Id;
            TenantId = command.TenantId;
            LegalName = command.LegalName;
            DisplayName = command.DisplayName;
            Address = command.Address;
            Phone = command.Phone;
            Email = command.Email;
            Active = command.Active;
        }
        protected void When(ServiceSubscriberLegalNameChanged @event)
        {
            LegalName = @event.LegalName;
        }
        protected void When(ServiceSubscriberDisplayNameChanged @event)
        {
            DisplayName = @event.DisplayName;
        }
        protected void When(ServiceSubscriberAddressChanged @event)
        {
            Address = @event.Address;
        }
        protected void When(ServiceSubscriberPhoneChanged @event)
        {
            Phone = @event.Phone;
        }
        protected void When(ServiceSubscriberEmailChanged @event)
        {
            Email = @event.Email;
        }
        protected void When(ServiceSubscriberActivated @event)
        {
            Active = true;
        }
        protected void When(ServiceSubscriberDeactivated @event)
        {
            Active = false;
        }
        #endregion
        
        #region SnapshotFunctionality
        public ServiceSubscriberSnapshot GetSnapshot()
        {
            var snapshotVersion = this.Version + Changes.Count;
            return new ServiceSubscriberSnapshot(this.Id, this.TenantId, this.LegalName, this.DisplayName,
                this.Address,this.Phone, this.Email, this.Active, snapshotVersion);
        }
        
        public ServiceSubscriber(ServiceSubscriberSnapshot snapshot, IEnumerable<IEvent> events)
        {
            Id = snapshot.Id;
            TenantId = snapshot.ServiceSubscriberId;
            Address = snapshot.Address;
            Email = snapshot.Email;
            Phone = snapshot.Phone;
            DisplayName = snapshot.DisplayName;
            LegalName = snapshot.LegalName;
            Active = snapshot.Active;
            
            Version = snapshot.Version;
            Changes = new List<IEvent>();
            
            foreach (var @event in events)
            {
                Mutate(@event);
                Version += 1;
            }
        }
        #endregion
    }
}