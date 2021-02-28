using System;
using System.Collections.Generic;
using System.Linq;
using ES.Domain;
using Ezley.Events;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.OrderSystem
{
    public class Customer: AggregateBase
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set;}
        public string LastName { get; private set; }
        public string MiddleInitial { get; private set; }
        
        public string MiddleName { get; private set; }
        
        public Customer(IEnumerable<IEvent> events): base(events)
        {
        }
        
        public Customer(Guid id,string firstName, string lastName, string middleName)
        {
            if (middleName.Length > 45)
                throw new ApplicationException("Middle Name can't be more than 45 char.");
            
            // Apply(new CustomerRegistered(id, firstName, lastName, middleInitial));
            Apply(new CustomerRegistered(id, firstName, lastName, "", middleName));
        }
  
        public void ChangeFirstName(string firstName)
        {
            if(FirstName != firstName)
                Apply(new CustomerFirstNameChanged(Id, firstName));
        }
        
        public void ChangeLastName(string lastName)
        {
            if(LastName != lastName)
                Apply(new CustomerLastNameChanged(Id, lastName));
        }
        
        // public void ChangeMiddleInitial(string middleInitial)
        // {
        //     if (middleInitial.Length > 1)
        //         throw new ApplicationException("Middle Initial can't be more than 1 char.");
        //     
        //     if(MiddleInitial != middleInitial)
        //         Apply(new  CustomerMiddleInitialChanged(Id, middleInitial));
        // }

        public void ChangeMiddleName(string middleName)
        {
            if (middleName.Length > 45)
                throw new ApplicationException("Middle Name can't be more than 45 char.");
            
            Apply(new CustomerMiddleNameChanged(Id, middleName));

        }
        
        // Event handlers
        protected override void Mutate(IEvent @event)
        {
            ((dynamic) this).MutateWhen((dynamic) @event);
        }
        
        protected void MutateWhen(CustomerRegistered @event)
        {
            Id = @event.Id;
            FirstName = @event.FirstName;
            LastName = @event.LastName;
            MiddleInitial = @event.MiddleInitial;
            MiddleName = @event.MiddleName;
        }

        protected void MutateWhen(CustomerFirstNameChanged @event)
        {
            FirstName = @event.FirstName;
        }

        protected void MutateWhen(CustomerLastNameChanged @event)
        {
            LastName = @event.LastName;
        }

        protected void MutateWhen(CustomerMiddleInitialChanged @event)
        {
            MiddleInitial = @event.MiddleInitial;
        }
        
        protected void MutateWhen(CustomerMiddleNameChanged @event)
        {
            MiddleName = @event.MiddleName;
        }
    }
}