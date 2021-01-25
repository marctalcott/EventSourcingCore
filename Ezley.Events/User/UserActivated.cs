using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
    public class UserActivated:EventBase
    {
        public Guid Id { get; private set; }
        public string DisplayName { get; private set; }

        public UserActivated(Guid id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }
    }
}