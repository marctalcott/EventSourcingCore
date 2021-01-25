using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
    public class UserDisplayNameChanged:EventBase
    {
        public Guid Id { get; private set; }
        public string DisplayName { get; private set; }

        public UserDisplayNameChanged(Guid id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }
    }
}