using System;
using System.Collections.Generic;
using System.Linq;
using ES.Domain;
using Ezley.Events;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.OrderSystem
{
    public class Order: AggregateBase
    {
        public Guid Id { get; private set; }
        public string OrderName { get; private set;}
        public List< OrderItem> Items { get; private set;}
        public Order(IEnumerable<IEvent> events): base(events)
        {
        }
        
        public Order(Guid id,string orderName, List<OrderItem> items)
        {
            Apply(new OrderPlaced(id, orderName, items));
        }

        public void AddItem(OrderItem item)
        {
            Apply(new OrderItemAdded(this.Id, item));
           
        }
        public void RemoveItem(OrderItem item)
        {
            if (Items.Any(x => x.Name == item.Name))
            {
                Apply(new OrderItemRemoved(this.Id, item));
            }
        }
        // Event handlers
        protected override void Mutate(IEvent @event)
        {
            ((dynamic) this).MutateWhen((dynamic) @event);
        }
        protected void MutateWhen(OrderPlaced @event)
        {
            Id = @event.Id;
            OrderName = @event.OrderName;
            Items = @event.Items;
        }

        protected void MutateWhen(OrderItemAdded @event)
        {
            // build new list
            var items = new List<OrderItem>();
            items.AddRange(Items);
            items.Add(@event.Item);
            // set to list with new item
            Items = items;
        }  
        protected void MutateWhen(OrderItemRemoved @event)
        {
            // build new list
            var items = new List<OrderItem>();
            items.AddRange(Items.Where(x => x.Name != OrderName));
            // set to list without item
            Items = items;
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