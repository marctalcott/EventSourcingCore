using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
    public class TenantDisplayNameChanged:EventBase
    {
        public Guid Id { get; private set; }
        public string DisplayName { get; private set; }

        public TenantDisplayNameChanged(Guid id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }
    }
}