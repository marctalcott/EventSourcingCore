using System;
using System.Collections.Generic;
using ES.Domain;
using Ezley.Events;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Domain.CRM
{
    public class User: AggregateBase
    {
        public Guid Id { get; private set; }
        public Guid TenantId { get; private set; }
        public PersonName PersonName { get; private set; }
        public string DisplayName { get; private set; }
        public Address Address { get; private set; }
        public Phone Phone { get; private set; }
        public Email Email { get; private set; }
        public bool Active { get; private set; }
        public string Auth0Id { get; private set; }

        public User(IEnumerable<IEvent> events): base(events)
        {
        }
        public User(Guid id, Guid TenantId, PersonName personName, string displayName,
            Address address, Phone phone, Email email, bool active = true)
        {
            Apply(new UserRegistered(id, TenantId, personName,displayName,address,phone,email, active));
        }
        public void ChangeTenantId(Guid TenantId)
        {
            if (TenantId != Guid.Empty)
            {
                throw new ApplicationException("Can't change user's service provider after it has been set.");
            }

            this.Apply(new UserTenantIdChanged(this.Id, TenantId));
        }
        public void ChangePersonName(PersonName personName)
        {
            if(PersonName != personName)
                this.Apply(new UserPersonNameChanged(this.Id, personName));
        }
        public void ChangeDisplayName(string displayName)
        {
            if(DisplayName != displayName)
                this.Apply(new UserDisplayNameChanged(this.Id, displayName));
        }
        public void ChangeAddress(Address address)
        {
            if(!Equals(Address, address))
                this.Apply(new UserAddressChanged(this.Id, address));
        }
        public void ChangePhone(Phone phone)
        {
            if(!Equals(Phone, phone))
                this.Apply(new UserPhoneChanged(this.Id, phone));
        }
        public void ChangeEmail(Email email)
        {
            if(!Equals(Email, email))
                this.Apply(new UserEmailChanged(this.Id, email));
        }
        public void ChangeAuth0Id(string auth0Id)
        {
            if(Auth0Id != auth0Id)
                this.Apply(new UserAuth0IdChanged(this.Id, auth0Id));
        }
         
        public void Activate()
        {
            if(!Active)
                this.Apply(new UserActivated(this.Id, this.DisplayName));
        }

        public void Deactivate()
        {
            if(Active)
                this.Apply(new UserDeactivated(this.Id, this.DisplayName));
        }
        
        
        #region WhenEvent_ApplyEventsOnly
        protected override void Mutate(IEvent @event)
        {
            ((dynamic) this).When((dynamic) @event);
        }
        protected void When(UserRegistered @event)
        {
            Id = @event.Id;
            TenantId = @event.TenantId;
            PersonName = @event.PersonName;
            DisplayName = @event.DisplayName;
            Address = @event.Address;
            Phone = @event.Phone;
            Email = @event.Email;
            Active = @event.Active;
        }
        protected void When(UserTenantIdChanged @event)
        {
            TenantId = @event.TenantId;
        }
        protected void When(UserPersonNameChanged @event)
        {
            PersonName = @event.PersonName;
        }
        protected void When(UserDisplayNameChanged @event)
        {
            DisplayName = @event.DisplayName;
        }
        protected void When(UserAddressChanged @event)
        {
            Address = @event.Address;
        }
        protected void When(UserPhoneChanged @event)
        {
            Phone = @event.Phone;
        }
        protected void When(UserEmailChanged @event)
        {
            Email = @event.Email;
        }
        protected void When(UserAuth0IdChanged @event)
        {
            Auth0Id = @event.Auth0Id;
        }
        protected void When(UserActivated @event)
        {
            Active = true;
        }
        
        protected void When(UserDeactivated @event)
        {
            Active = false;
        }
        #endregion
    }
}