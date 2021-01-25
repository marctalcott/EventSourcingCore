using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Events
{
    public class ServiceSubscriberPhoneChanged:EventBase
    {
        public Guid Id { get; private set; }
        public Phone Phone { get; private set; }

        public ServiceSubscriberPhoneChanged(Guid id, Phone phone)
        {
            Id = id;
            Phone = phone;
        }
    }
}