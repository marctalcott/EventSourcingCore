using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
    public class ServiceSubscriberLegalNameChanged:EventBase
    {
        public Guid Id { get; private set; }
        public Guid TenantId { get; private set; }
        public string LegalName { get; private set; }

        public ServiceSubscriberLegalNameChanged(
            Guid id, Guid tenantId, string legalName)
        {
            Id = id;
            TenantId = tenantId;
            LegalName = legalName;
        }
    }
}