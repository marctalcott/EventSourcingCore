using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Events
{
    public class UserRegistered: EventBase
    {
        public Guid Id { get; private set; }
        public Guid TenantId { get; private set; }
        public PersonName PersonName { get; private set; }
        public string DisplayName { get; private set; }
        public Address Address { get; private set; }
        public Phone Phone { get; private set; }
        public Email Email { get; private set; }
        public bool Active { get; private set; }

        public UserRegistered(
            Guid id, Guid tenantId, PersonName personName, string displayName,
            Address address, Phone phone, Email email, bool active)
        {
            Id = id;
            TenantId = tenantId;
            PersonName = personName;
            DisplayName = displayName;
            Address = address;
            Phone = phone;
            Email = email;
            Active = active;
        }
    }
}