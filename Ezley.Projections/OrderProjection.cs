using System;
using System.Collections.Generic;
using System.Linq;
using Ezley.Events;
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
            RegisterHandler<OrderItemAdded>(WhenItemAdded);
            RegisterHandler<OrderItemRemoved>(WhenItemRemoved);
        }
        
        private void WhenOrderPlaced(OrderPlaced e, OrderView view)
        {
            view.Id = e.Id;
            view.OrderName = e.OrderName;
            view.Items = e.Items;
        }

        private void WhenItemAdded(OrderItemAdded e, OrderView view)
        {
            // build new list
            var items = new List<OrderItem>();
            items.AddRange(view.Items);
            items.Add(e.Item);
            
            // set to list with new item
            view.Items = items;
        }
        private void WhenItemRemoved(OrderItemRemoved e, OrderView view)
        {
            // build new list
            var items = new List<OrderItem>();
            items.AddRange(view.Items.Where(x => x.Name != e.Name));
            
            // set to list without item
            view.Items = items;
        }
    }    
}