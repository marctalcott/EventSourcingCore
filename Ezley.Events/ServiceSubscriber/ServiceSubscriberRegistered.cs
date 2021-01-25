using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Events
{
    public class ServiceSubscriberRegistered:EventBase
    {
        public Guid Id { get; private set; }
        public Guid TenantId { get; private set; }
        public string LegalName { get; private set; }
        public string DisplayName { get; private set; }
        public Address Address { get; private set; }
        public Phone Phone { get; private set; }
        public Email Email { get; private set; }
        public bool Active { get; private set; }

        public ServiceSubscriberRegistered(Guid id,Guid tenantId, string legalName, string displayName,
            Address address, Phone phone, Email email, bool active = true)
        {
            Id = id;
            TenantId = tenantId;
            LegalName = legalName;
            DisplayName = displayName;
            Address = address;
            Phone = phone;
            Email = email;
            Active = active;
        }
    }
}