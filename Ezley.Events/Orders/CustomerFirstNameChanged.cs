using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
   
    public class CustomerFirstNameChanged: EventBase
    {
        public Guid Id { get; }
        public string FirstName { get; }
 
        public CustomerFirstNameChanged(Guid id, string firstName)
        {
            Id = id;
            FirstName = firstName;
        }
    }
}