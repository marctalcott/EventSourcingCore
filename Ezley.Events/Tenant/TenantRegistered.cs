using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Events
{
    public class TenantRegistered:EventBase
    {
        public Guid Id { get; private set; }
        public string LegalName { get; private set; }
        public string DisplayName { get; private set; }
        public Address Address { get; private set; }
        public Phone Phone { get; private set; }
        public Email Email { get; private set; }
        public bool Active { get; private set; }

        public TenantRegistered(Guid id,string legalName, string displayName,
            Address address, Phone phone, Email email, bool active)
        {
            Id = id;
            LegalName = legalName;
            DisplayName = displayName;
            Address = address;
            Phone = phone;
            Email = email;
            Active = active;
        }
    }
}