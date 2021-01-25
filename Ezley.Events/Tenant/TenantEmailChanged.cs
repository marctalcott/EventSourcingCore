using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Events
{
    public class TenantEmailChanged:EventBase
    {
        public Guid Id { get; private set; }
        public Email Email { get; private set; }

        public TenantEmailChanged(Guid id, Email email)
        {
            Id = id;
            Email = email;
        }
    }
}