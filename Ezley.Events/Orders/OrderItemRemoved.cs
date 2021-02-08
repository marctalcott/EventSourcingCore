using System;
using System.Collections.Generic;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Events
{
   
    public class OrderItemRemoved: EventBase
    {
        public Guid Id { get; }

        public OrderItem Item { get; }
        

        public OrderItemRemoved(Guid id,  OrderItem item)
        {
            Id = id;
            Item = item;
        }

    }
}