using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
    public class TenantLegalNameChanged:EventBase
    {
        public Guid Id { get; private set; }
        public string LegalName { get; private set; }

        public TenantLegalNameChanged(Guid id, string legalName)
        {
            Id = id;
            LegalName = legalName;
        }
    }
}