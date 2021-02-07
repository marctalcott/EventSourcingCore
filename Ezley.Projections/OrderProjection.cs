using System;
using System.Collections.Generic;
using Ezley.Events.Orders;
using Ezley.ProjectionStore;
using Ezley.ValueObjects;

namespace Ezley.Projections
{
    public class OrderView
    {
        public Guid Id { get; set; }
        public string OrderName { get; set; }
        public List<OrderItem> Items { get; set; }
    }
    
    public class OrderProjection : Projection<OrderView>
    {
        public OrderProjection()
        {
            RegisterHandler<OrderPlaced>(WhenOrderPlaced);
        }
        
        private void WhenOrderPlaced(OrderPlaced e, OrderView view)
        {
            view.Id = e.Id;
            view.OrderName = e.OrderName;
            view.Items = e.Items;
        }
    }    
}