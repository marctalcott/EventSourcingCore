using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
    public class ServiceSubscriberActivated:EventBase
    {
        public Guid Id { get; private set; }
        public Guid TenantId { get; private set; }
        public string LegalName { get; private set; }
        public string DisplayName { get; private set; }
        public ServiceSubscriberActivated(Guid id, 
            Guid tenantId, string legalName, string displayName)
        {
            Id = id;
            TenantId = tenantId;
            LegalName = legalName;
            DisplayName = displayName;
        }
    }
}