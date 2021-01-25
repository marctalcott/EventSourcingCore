using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
    public class TenantActivated:EventBase
    {
        public Guid Id { get; private set; }
        public string LegalName { get; private set; }
        public string DisplayName { get; private set; }
        public TenantActivated(Guid id, string legalName, 
            string displayName)
        {
            Id = id;
            LegalName = legalName;
            DisplayName = displayName;
        }
    }
}