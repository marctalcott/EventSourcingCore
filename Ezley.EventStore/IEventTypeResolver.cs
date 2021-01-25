using System;

namespace Ezley.EventSourcing
{
    public interface IEventTypeResolver
    {
        Type GetEventType(string typeName);
    }
}