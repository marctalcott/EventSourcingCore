using System;
using System.Collections.Generic;
using Ezley.Events;
using Ezley.EventSourcing;
using Ezley.ProjectionStore;

namespace Ezley.Projections
{
    public class OrderHeaderView
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }


        public OrderHeaderView(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class PendingOrdersView
    {
        public List<OrderHeaderView> Items { get; set; }

        public PendingOrdersView()
        {
            Items = new List<OrderHeaderView>();
        }
    }
    
    public class PendingOrdersProjection : Projection<PendingOrdersView>
    {
        public override string[] GetViewNames(string streamId, IEvent @event) =>
            new string[] { "PendingOrdersView"};
        public PendingOrdersProjection()
        {
            RegisterHandler<OrderPlaced>(WhenOrderPlaced);
        }
        
        private void WhenOrderPlaced(OrderPlaced e, PendingOrdersView view)
        {
            var order = new OrderHeaderView(e.Id, e.OrderName);
            view.Items.Add(order);
        }
    }    
}