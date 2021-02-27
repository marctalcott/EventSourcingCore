using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
   
    public class CustomerLastNameChanged: EventBase
    {
        public Guid Id { get; }
        public string LastName { get; }
 
        public CustomerLastNameChanged(Guid id, string lastName)
        {
            Id = id;
            LastName = lastName;
        }
    }
}