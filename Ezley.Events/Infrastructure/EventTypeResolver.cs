using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
    public class EventTypeResolver: IEventTypeResolver
    {
        private string _namespace = "Ezley.Events";
        public Type GetEventType(string typeName)
        {
            return Type.GetType($"{_namespace}.{typeName}");
        }
    }
}