using System;
using System.Linq;
using System.Threading.Tasks;
using Ezley.EventSourcing;
using Ezley.EventStore;
using Ezley.SnapshotStore;

namespace Ezley.OrderSystem.Repositories
{
    public interface IOrderSystemRepository
    {
        Task<Order> LoadOrder(Guid id);
        Task<bool> SaveOrder(EventUserInfo eventUserInfo, Order aggregate, OrderSnapshot snapshot = null);
        Task<bool> SaveOrderSnapshot(OrderSnapshot snapshot);
        
        Task<Customer> LoadCustomer(Guid id);
        Task<bool> SaveCustomer(EventUserInfo eventUserInfo, Customer aggregate);
        
    }

    public class OrderSystemRepository : IOrderSystemRepository
    {
       private readonly IEventStore _eventStore;
        private readonly ISnapshotStore _snapshotStore;
        public OrderSystemRepository(IEventStore eventStore, ISnapshotStore snapshotStore)
        {
            _eventStore = eventStore;
            _snapshotStore = snapshotStore;
        }
       #region Order
        public async Task<Order> LoadOrder(Guid id)
        {
            // You could add logic here to make sure you are only getting items you should
            // for instance if you restrict access by tenantId, take in a TenantId parameter and
            // only return if the tenantId matches.
            if (_snapshotStore != null)
            {
                var snapshotStreamId =  $"Order:{id}";
                var snapshot = await _snapshotStore.LoadSnapshotAsync(snapshotStreamId);
                if (snapshot != null)
                {
                    var streamTail = await _eventStore.LoadStreamAsync(id.ToString(), snapshot.Version + 1);

                    var orderSnapshot = snapshot.SnapshotData.ToObject<OrderSnapshot>();
                    return new Order(
                        orderSnapshot,
                        streamTail.Events);
                }
            }
            
           var stream = await _eventStore.LoadStreamAsyncOrThrowNotFound(id.ToString());
           return new Order(stream.Events);
        }
        
        public async Task<bool> SaveOrder(EventUserInfo eventUserInfo, Order aggregate, OrderSnapshot snapshot = null)
        {
            if (aggregate.Changes.Any())
            {
                var streamId = aggregate.Id.ToString();
                
                // save all events
                bool savedEvents = await _eventStore.AppendToStreamAsync(eventUserInfo, streamId,
                    aggregate.Version,
                    aggregate.Changes);
                
                // save snapshot
                if (savedEvents && snapshot != null && _snapshotStore != null)
                {
                    await SaveOrderSnapshot(snapshot);
                }
                return savedEvents;
            }
            return true;
        }

        public async Task<bool> SaveOrderSnapshot(OrderSnapshot snapshot)
        {
            var streamId = $"Order:{snapshot.Id.ToString()}";
            await _snapshotStore
                .SaveSnapshotAsync(streamId , snapshot.Version, snapshot);
            return true;
        }
        #endregion
         
        
        #region Customer
        public async Task<Customer> LoadCustomer(Guid id)   
        {
            var stream = await _eventStore.LoadStreamAsyncOrThrowNotFound(id.ToString());
            return new Customer(stream.Events);
        }

        public async Task<bool> SaveCustomer(EventUserInfo eventUserInfo, Customer aggregate)
        {
            if (!aggregate.Changes.Any())
            {
                return true;
            }

            var streamId = aggregate.Id.ToString();
            return await _eventStore.AppendToStreamAsync(eventUserInfo, streamId,
                    aggregate.Version,
                    aggregate.Changes);
        }
        #endregion
         
    }
}