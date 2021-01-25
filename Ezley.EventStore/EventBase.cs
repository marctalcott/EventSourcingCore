using System;
using System.Diagnostics;

namespace Ezley.EventSourcing
{
    [DebuggerStepThrough]
    public abstract class EventBase : IEvent
    {
        public DateTime Timestamp { get; } = DateTime.UtcNow;

    }
}