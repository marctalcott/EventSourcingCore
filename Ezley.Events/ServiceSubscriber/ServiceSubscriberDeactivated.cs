using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
    public class ServiceSubscriberDeactivated:EventBase
    {
        public Guid Id { get; private set; }
        public Guid TenantId { get; private set; }
        public string LegalName { get; private set; }
        public string DisplayName { get; private set; }

        public ServiceSubscriberDeactivated(Guid id, 
            Guid tenantId, string legalName, string displayName)
        {
            Id = id;
            TenantId = tenantId;
            LegalName = legalName;
            DisplayName = displayName;
        }
    }
}