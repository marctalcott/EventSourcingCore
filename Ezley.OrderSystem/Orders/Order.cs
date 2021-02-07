using System;
using System.Collections.Generic;
using ES.Domain;
using Ezley.Events;
using Ezley.Events.Orders;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.OrderSystem
{
    public class Order: AggregateBase
    {
        public Guid Id { get; }
        public string OrderName { get; }
        public List< OrderItem> Items { get; }
        public Order(IEnumerable<IEvent> events): base(events)
        {
        }
        
        public Order(Guid id,string orderName, List<OrderItem> items)
        {
            Apply(new OrderPlaced(id, orderName, items));
        }
        
        
        // Event handlers
        protected override void Mutate(IEvent @event)
        {
            ((dynamic) this).When((dynamic) @event);
        }
         
        
        #region SnapshotFunctionality
        public OrderSnapshot GetSnapshot()
        {
            var snapshotVersion = this.Version + Changes.Count;
            return new OrderSnapshot(this.Id,this.OrderName, this.Items, snapshotVersion);
        }
        
        public Order(OrderSnapshot snapshot, IEnumerable<IEvent> events)
        {
            Id = snapshot.Id;
            OrderName = snapshot.OrderName;
            Items = snapshot.Items;

            Version = snapshot.Version;
            Changes = new List<IEvent>();
            
            foreach (var @event in events)
            {
                Mutate(@event);
                Version += 1;
            }
        }
        #endregion
    }

}