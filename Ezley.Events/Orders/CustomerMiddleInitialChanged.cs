using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
   
    public class CustomerMiddleInitialChanged: EventBase
    {
        public Guid Id { get; }
        public string MiddleInitial { get; }
 
        public CustomerMiddleInitialChanged(Guid id, string middleInitial)
        {
            Id = id;
            MiddleInitial = middleInitial;
        }
    }
}