using System;
using Ezley.EventSourcing;
using Ezley.Events;
using Ezley.ValueObjects;
using Ezley.ProjectionStore;

namespace Ezley.Projections
{
    public class UserView
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public PersonName PersonName { get; set; }
        public string DisplayName { get; set; }
        public Address Address { get; set; }
        public Phone Phone { get; set; }
        public Email Email { get; set; }
        public bool Active { get; set; }
        public string Auth0Id { get; set; }
    }

    public class UserProjection : Projection<UserView>
    {
        public UserProjection()
        {
            RegisterHandler<UserRegistered>(WhenUserRegistered);
            RegisterHandler<UserActivated>(WhenUserActivated);
            RegisterHandler<UserDeactivated>(WhenUserDeactivated);
            RegisterHandler<UserTenantIdChanged>(WhenUserTenantIdChanged);
            RegisterHandler<UserAddressChanged>(WhenUserAddressChanged);
            RegisterHandler<UserPhoneChanged>(WhenUserPhoneChanged);
            RegisterHandler<UserEmailChanged>(WhenUserEmailChanged);
            RegisterHandler<UserPersonNameChanged>(WhenUserPersonNameChanged);
            RegisterHandler<UserDisplayNameChanged>(WhenUserDisplayNameChanged);
        }
        public override string[] GetViewNames(string streamId, IEvent @event)
        {
            Guid userId = GetUserId((dynamic)@event);
            string eventName = nameof(UserView);
            var names = new string[] {$"{eventName}:{userId}"};
            return names;
        }
        
        private Guid GetUserId(UserRegistered @event)
        {
            return @event.Id;
        }
        private Guid GetUserId(UserActivated @event)
        {
            return @event.Id;
        }
        private Guid GetUserId(UserDeactivated @event)
        {
            return @event.Id;
        }
        private Guid GetUserId(UserTenantIdChanged @event)
        {
            return @event.Id;
        }
        private Guid GetUserId(UserAddressChanged @event)
        {
            return @event.Id;
        }
        private Guid GetUserId(UserPhoneChanged @event)
        {
            return @event.Id;
        }
        private Guid GetUserId(UserEmailChanged @event)
        {
            return @event.Id;
        }
        private Guid GetUserId(UserPersonNameChanged @event)
        {
            return @event.Id;
        }

        private Guid GetUserId(UserDisplayNameChanged @event)
        {
            return @event.Id;
        }
        
        private void WhenUserRegistered(UserRegistered e, UserView view)
        {
            view.Id = e.Id;
            view.TenantId = e.TenantId;
            view.PersonName = e.PersonName;
            view.DisplayName = e.DisplayName;
            view.Address = e.Address;
            view.Phone = e.Phone;
            view.Email = e.Email;
            view.Active = e.Active;
        }
        private void WhenUserTenantIdChanged(UserTenantIdChanged e, UserView view)
        {
            view.TenantId = e.TenantId;
        }
        private void WhenUserActivated(UserActivated e, UserView view)
        {
            view.Active = true;
        }

        private void WhenUserDeactivated(UserDeactivated e, UserView view)
        {
            view.Active = false;
        }
        private void WhenUserAddressChanged(UserAddressChanged e, UserView view)
        {
            view.Address = e.Address;
        }
        private void WhenUserEmailChanged(UserEmailChanged e, UserView view)
        {
            view.Email = e.Email;
        }
        private void WhenUserPhoneChanged(UserPhoneChanged e, UserView view)
        {
            view.Phone = e.Phone;
        }
        private void WhenUserPersonNameChanged(UserPersonNameChanged e, UserView view)
        {
            view.PersonName = e.PersonName;
        }
        private void WhenUserDisplayNameChanged(UserDisplayNameChanged e, UserView view)
        {
            view.DisplayName = e.DisplayName;
        }

    }    
}