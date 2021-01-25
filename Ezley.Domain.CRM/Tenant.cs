using System;
using System.Collections.Generic;
using ES.Domain;
using Ezley.Events;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Domain.CRM
{
    public class Tenant: AggregateBase
    {
        public Guid Id { get; private set; }
        public string LegalName { get; private set; }
        public string DisplayName { get; private set; }
        public Address Address { get; private set; }
        public Phone Phone { get; private set; }
        public Email Email { get; private set; }
        public bool Active { get; private set; }

        public Tenant(IEnumerable<IEvent> events): base(events)
        {
        }
        public Tenant(Guid id,string legalName, string displayName,
            Address address, Phone phone, Email email, bool active = true)
        {
            Apply(new TenantRegistered(id, legalName,displayName,address,phone,email, active));
        }

        #region AggCommands_CreateAndApplyEvents
        public void ChangeLegalName(string legalName)
        {
            if(LegalName != legalName)
                this.Apply(new TenantLegalNameChanged(this.Id, legalName));
        }
        public void ChangeDisplayName(string displayName)
        {
            if(DisplayName != displayName)
                this.Apply(new TenantDisplayNameChanged(this.Id, displayName));
        }
        public void ChangeAddress(Address address)
        {
            if(!Equals(Address, address))
                this.Apply(new TenantAddressChanged(this.Id, address));
        }
        public void ChangePhone(Phone phone)
        {
            if(!Equals(Phone, phone))
                this.Apply(new TenantPhoneChanged(this.Id, phone));
        }
        public void ChangeEmail(Email email)
        {
            if(!Equals(Email, email))
                this.Apply(new TenantEmailChanged(this.Id, email));
        }

        public void Activate()
        {
            if(!Active)
                this.Apply(new TenantActivated(this.Id,  this.LegalName, this.DisplayName));
        }

        public void Deactivate()
        {
            if(Active)
                this.Apply(new TenantDeactivated(this.Id, this.LegalName, this.DisplayName));
        }
        #endregion
        
        #region WhenEvent_ApplyEventsOnly
        protected override void Mutate(IEvent @event)
        {
            ((dynamic) this).When((dynamic) @event);
        }
        protected void When(TenantRegistered @event)
        {
            Id = @event.Id;
            LegalName = @event.LegalName;
            DisplayName = @event.DisplayName;
            Address = @event.Address;
            Phone = @event.Phone;
            Email = @event.Email;
            Active = @event.Active;
        }
        protected void When(TenantLegalNameChanged @event)
        {
            LegalName = @event.LegalName;
        }
        protected void When(TenantDisplayNameChanged @event)
        {
            DisplayName = @event.DisplayName;
        }
        protected void When(TenantAddressChanged @event)
        {
            Address = @event.Address;
        }
        protected void When(TenantPhoneChanged @event)
        {
            Phone = @event.Phone;
        }
        protected void When(TenantEmailChanged @event)
        {
            Email = @event.Email;
        }

        protected void When(TenantActivated @event)
        {
            Active = true;
        }
        
        protected void When(TenantDeactivated @event)
        {
            Active = false;
        }
        #endregion
        
        #region SnapshotFunctionality
        public TenantSnapshot GetSnapshot()
        {
            var snapshotVersion = this.Version + Changes.Count;
            return new TenantSnapshot(this.Id,this.LegalName, this.DisplayName,
                this.Address,this.Phone, this.Email, this.Active, snapshotVersion);
        }
        
        public Tenant(TenantSnapshot snapshot, IEnumerable<IEvent> events)
        {
            Id = snapshot.Id;
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