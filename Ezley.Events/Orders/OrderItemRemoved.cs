using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
   
    public class OrderItemRemoved: EventBase
    {
        public Guid Id { get; }

        public string Name { get; }
        

        public OrderItemRemoved(Guid id,  string name)
        {
            Id = id;
            Name = name;
        }

    }
}