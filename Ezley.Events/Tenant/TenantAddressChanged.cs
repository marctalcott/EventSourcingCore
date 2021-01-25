using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Events
{
    public class TenantAddressChanged:EventBase
    {
        public Guid Id { get; private set; }
        public Address Address { get; private set; }

        public TenantAddressChanged(Guid id, Address address)
        {
            Id = id;
            Address = address;
        }
    }
}