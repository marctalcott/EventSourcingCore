using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
    public class CustomerMiddleNameChanged: EventBase
    {
        public Guid Id { get; }
        public string MiddleName { get; }
 
        public CustomerMiddleNameChanged(Guid id, string middleName)
        {
            Id = id;
            MiddleName = middleName;
        }
    }
}