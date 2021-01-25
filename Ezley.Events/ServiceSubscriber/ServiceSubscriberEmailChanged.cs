using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Events
{
    public class ServiceSubscriberEmailChanged:EventBase
    {
        public Guid Id { get; private set; }
        public Email Email { get; private set; }

        public ServiceSubscriberEmailChanged(Guid id, Email email)
        {
            Id = id;
            Email = email;
        }
    }
}