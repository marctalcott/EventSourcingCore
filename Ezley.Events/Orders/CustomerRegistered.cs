using System;
using System.Collections.Generic;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Events
{
   
    public class CustomerRegistered: EventBase
    {
        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string MiddleInitial { get; }
        public string MiddleName { get; }

        public CustomerRegistered(Guid id, string firstName, string lastName, 
            string middleInitial,
            string middleName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            MiddleInitial = middleInitial;
            MiddleName = middleName;
        }
    }
}