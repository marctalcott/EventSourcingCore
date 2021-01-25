using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Events
{
    public class TenantPhoneChanged:EventBase
    {
        public Guid Id { get; private set; }
        public Phone Phone { get; private set; }

        public TenantPhoneChanged(Guid id, Phone phone)
        {
            Id = id;
            Phone = phone;
        }
    }
}