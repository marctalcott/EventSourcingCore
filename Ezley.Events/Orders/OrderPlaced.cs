using System;
using System.Collections.Generic;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Events
{
   
    public class OrderPlaced: EventBase
    {
        public Guid Id { get; }
        public string OrderName { get; }
        public List<OrderItem> Items { get; }

        public OrderPlaced(Guid id, string orderName, List<OrderItem> items)
        {
            Id = id;
            OrderName = orderName;
            Items = items;
        }
    }
}