using System;
using System.Collections.Generic;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Events
{
   
    public class OrderItemAdded: EventBase
    {
        public Guid Id { get; }

        public OrderItem Item { get; }
        

        public OrderItemAdded(Guid id,  OrderItem item)
        {
            Id = id;
            Item = item;
        }

    }
}