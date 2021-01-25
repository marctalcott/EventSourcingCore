using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
    public class ServiceSubscriberDisplayNameChanged:EventBase
    {
        public Guid Id { get; private set; } 
        public Guid TenantId { get; private set; }
        public string DisplayName { get; private set; }

        public ServiceSubscriberDisplayNameChanged(
            Guid id, Guid tenantId,
            string displayName)
        {
            Id = id;
            TenantId = tenantId;
            DisplayName = displayName;
        }
    }
}