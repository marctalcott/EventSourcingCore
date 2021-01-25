using System;
using System.Linq;
using System.Threading.Tasks;
using Ezley.EventSourcing;
using Ezley.EventStore;
using Ezley.SnapshotStore;

namespace Ezley.Domain.CRM.Repositories
{
   public interface ICrmRepository
    {
        Task<Tenant> LoadTenant(Guid id);
        Task<bool> SaveTenant(EventUserInfo eventUserInfo, Tenant tenant, TenantSnapshot snapshot = null);

        Task<ServiceSubscriber> LoadServiceSubscriber(Guid id);

        Task<bool> SaveServiceSubscriber(
            EventUserInfo eventUserInfo,
            ServiceSubscriber aggregate,
            ServiceSubscriberSnapshot snapshot = null);
        
        Task<User> LoadUser(Guid id);
        Task<bool> SaveUser(EventUserInfo eventUserInfo, User user);
        
        Task<Auth0User> LoadAuth0User(string id);
        Task<bool> SaveAuth0User(EventUserInfo eventUserInfo, Auth0User user);
    }

    public class CrmRepository : ICrmRepository
    {
        private readonly IEventStore _eventStore;
        private readonly ISnapshotStore _snapshotStore;
        public CrmRepository(IEventStore eventStore, ISnapshotStore snapshotStore)
        {
            _eventStore = eventStore;
            _snapshotStore = snapshotStore;
        }
       #region Tenant
        public async Task<Tenant> LoadTenant(Guid id)
        {
            // You could add logic here to make sure you are only getting items you should
            // for instance if you restrict access by tenantId, take in a TenantId parameter and
            // only return if the tenantId matches.
            var snapshotStreamId =  $"Tenant:{id.ToString()}";
            var snapshot = await _snapshotStore.LoadSnapshotAsync(snapshotStreamId);
           
           if (snapshot != null)
           {
               var streamTail = await _eventStore.LoadStreamAsync(id.ToString(), snapshot.Version + 1);

               var TenantSnapshot = snapshot.SnapshotData.ToObject<TenantSnapshot>();
               return new Tenant(
                   TenantSnapshot,
                   streamTail.Events);
           }
           else
           {
               var stream = await _eventStore.LoadStreamAsyncOrThrowNotFound(id.ToString());
               return new Tenant(stream.Events);
           }
        }
        
        public async Task<bool> SaveTenant(EventUserInfo eventUserInfo, Tenant aggregate, TenantSnapshot snapshot = null)
        {
            if (aggregate.Changes.Any())
            {
                var streamId = aggregate.Id.ToString();
                
                // save all events
                bool savedEvents = await _eventStore.AppendToStreamAsync(eventUserInfo, streamId,
                    aggregate.Version,
                    aggregate.Changes);
                
                // save snapshot
                if (savedEvents && snapshot != null)
                {
                    await SaveTenantSnapshot(snapshot);
                }
                return savedEvents;
            }
            return true;
        }
        
        private async Task<bool> SaveTenantSnapshot(TenantSnapshot snapshot)
        {

            var streamId = $"Tenant:{snapshot.Id.ToString()}";
            await _snapshotStore
                .SaveSnapshotAsync(streamId , snapshot.Version, snapshot);
            return true;
        }
        #endregion
        
        #region ServiceSubscriber
        public async Task<ServiceSubscriber> LoadServiceSubscriber(Guid id)
        {
            // You could add logic here to make sure you are only getting items you should
            // for instance if you restrict access by tenantId, take in a TenantId parameter and
            // only return if the tenantId matches.
            var snapshotStreamId =  $"ServiceSubscriber:{id.ToString()}";
            var snapshot = await _snapshotStore.LoadSnapshotAsync(snapshotStreamId);
           
           if (snapshot != null)
           {
               var streamTail = await _eventStore.LoadStreamAsync(id.ToString(), snapshot.Version + 1);

               var serviceSubscriberSnapshot = snapshot.SnapshotData.ToObject<ServiceSubscriberSnapshot>();
               return new ServiceSubscriber(
                   serviceSubscriberSnapshot,
                   streamTail.Events);
           }
           else
           {
               var stream = await _eventStore.LoadStreamAsyncOrThrowNotFound(id.ToString());
               return new ServiceSubscriber(stream.Events);
           }
        }
        
        public async Task<bool> SaveServiceSubscriber(EventUserInfo eventUserInfo,
            ServiceSubscriber aggregate,
            ServiceSubscriberSnapshot snapshot = null)
        {
            if (aggregate.Changes.Any())
            {
                var streamId = aggregate.Id.ToString();
                
                // save all events
                bool savedEvents = await _eventStore.AppendToStreamAsync(
                    eventUserInfo,
                    streamId,
                    aggregate.Version,
                    aggregate.Changes);
                
                // save snapshot
                if (savedEvents && snapshot != null)
                {
                    await SaveServiceSubscriberSnapshot(snapshot);
                }
                return savedEvents;

            }
            return true;
        }
        
        private async Task<bool> SaveServiceSubscriberSnapshot(ServiceSubscriberSnapshot snapshot)
        {
            var streamId = $"ServiceSubscriber:{snapshot.Id.ToString()}";
            await _snapshotStore
                .SaveSnapshotAsync(streamId , snapshot.Version, snapshot);
            return true;
        }
        #endregion
        
        #region User
        public async Task<User> LoadUser(Guid id)
        {
            var snapshotStreamId =  $"User:{id.ToString()}";
            
            var stream = await _eventStore.LoadStreamAsyncOrThrowNotFound(id.ToString());
            return new User(stream.Events);
        }
        
        public async Task<bool> SaveUser(EventUserInfo eventUserInfo,
            User aggregate)
        {
            if (aggregate.Changes.Any())
            {
                var streamId = aggregate.Id.ToString();
                
                // save all events
                bool savedEvents = await _eventStore.AppendToStreamAsync(
                    eventUserInfo,
                    streamId,
                    aggregate.Version,
                    aggregate.Changes);
               
                return savedEvents;
            }
            return true;
        }
        #endregion
        
        #region Auth0User
        public async Task<Auth0User> LoadAuth0User(string id)
        {
            var snapshotStreamId =  $"Auth0User:{id}";
            
            var stream = await _eventStore.LoadStreamAsyncOrThrowNotFound(id.ToString());
            return new Auth0User(stream.Events);
        }
        
        public async Task<bool> SaveAuth0User(EventUserInfo eventUserInfo,
            Auth0User aggregate)
        {
            if (aggregate.Changes.Any())
            {
                var streamId = aggregate.Id.ToString();
                
                // save all events
                bool savedEvents = await _eventStore.AppendToStreamAsync(
                    eventUserInfo,
                    streamId,
                    aggregate.Version,
                    aggregate.Changes);
               
                return savedEvents;
            }
            return true;
        }
        #endregion
    }
}