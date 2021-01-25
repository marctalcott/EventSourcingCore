using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Events
{
    public class ServiceSubscriberAddressChanged:EventBase
    {
        public Guid Id { get; private set; }
        public Address Address { get; private set; }

        public ServiceSubscriberAddressChanged(Guid id, Address address)
        {
            Id = id;
            Address = address;
        }
    }
}