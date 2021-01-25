using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
    public class UserDeactivated:EventBase
    {
        public Guid Id { get; private set; }
        public string DisplayName { get; private set; }

        public UserDeactivated(Guid id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }
    }
}