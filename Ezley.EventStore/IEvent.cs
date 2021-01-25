using System;

namespace Ezley.EventSourcing
{
    public interface IEvent
    {
        DateTime Timestamp { get; }
    }
}