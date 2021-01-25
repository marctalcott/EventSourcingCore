using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
    public class UserTenantIdChanged:EventBase
    {
        public Guid Id { get; private set; }
        public Guid TenantId { get; private set; }

        public UserTenantIdChanged(Guid id, Guid tenantId)
        {
            Id = id;
            TenantId = tenantId;
        }
    }
}