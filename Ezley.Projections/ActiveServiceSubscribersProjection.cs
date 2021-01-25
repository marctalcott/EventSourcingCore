using System;
using System.Collections.Generic;
using System.Linq;
using Ezley.EventSourcing;
using Ezley.Events;
using Ezley.ProjectionStore;

namespace Ezley.Projections
{
    public class ActiveServiceSubscribersView
    {
        public List<ServiceSubscriberListItem> ServiceSubscribers { get; set; } = new List<ServiceSubscriberListItem>();
    }

    public class ServiceSubscriberListItem
    {
        public Guid Id { get; set; }
        public string LegalName { get; set; }
        public string DisplayName { get; set; }
    }

    public class ActiveServiceSubscribersProjection : Projection<ActiveServiceSubscribersView>
    {
        public ActiveServiceSubscribersProjection()
        {
            RegisterHandler<ServiceSubscriberRegistered>(WhenServiceSubscriberRegistered);
            RegisterHandler<ServiceSubscriberActivated>(WhenServiceSubscriberActivated);
            RegisterHandler<ServiceSubscriberDeactivated>(WhenServiceSubscriberDeactivated);
            RegisterHandler<ServiceSubscriberLegalNameChanged>(WhenServiceSubscriberLegalNameChanged);
            RegisterHandler<ServiceSubscriberDisplayNameChanged>(WhenServiceSubscriberDisplayNameChanged);
        }
        public override string[] GetViewNames(string streamId, IEvent @event)
        {
            Guid providerId = GetProviderId((dynamic)@event);
            string eventName = nameof(ActiveServiceSubscribersView);
            var names = new string[] {$"{eventName}:{providerId}"};
            return names;
        }
        
        public Guid GetProviderId(ServiceSubscriberRegistered @event)
        {
            return @event.TenantId;
        }
        public Guid GetProviderId(ServiceSubscriberActivated @event)
        {
            return @event.TenantId;
        }
        public Guid GetProviderId(ServiceSubscriberDeactivated @event)
        {
            return @event.TenantId;
        }
        public Guid GetProviderId(ServiceSubscriberLegalNameChanged @event)
        {
            return @event.TenantId;
        }
        public Guid GetProviderId(ServiceSubscriberDisplayNameChanged @event)
        {
            return @event.TenantId;
        }

        private void WhenServiceSubscriberRegistered(ServiceSubscriberRegistered e, ActiveServiceSubscribersView view)
        {
            if (e.Active)
            {
                var subscriber = new ServiceSubscriberListItem()
                {
                    Id = e.Id,
                    LegalName = e.LegalName,
                    DisplayName = e.DisplayName
                };
                view.ServiceSubscribers.Add(subscriber);
            }
        }

        private void WhenServiceSubscriberActivated(ServiceSubscriberActivated e, ActiveServiceSubscribersView view)
        {
            var subscriber = new ServiceSubscriberListItem()
            {
                Id = e.Id,
                LegalName = e.LegalName,
                DisplayName = e.DisplayName
            };
            view.ServiceSubscribers.Add(subscriber);
        }

        private void WhenServiceSubscriberDeactivated(ServiceSubscriberDeactivated e, ActiveServiceSubscribersView view)
        {
            if ( view.ServiceSubscribers.Any(x => x.Id == e.Id))
            {
                view.ServiceSubscribers.Remove(view.ServiceSubscribers.First(x => x.Id == e.Id));
            }
        }

        private void WhenServiceSubscriberLegalNameChanged(ServiceSubscriberLegalNameChanged e, ActiveServiceSubscribersView view)
        {
            if ( view.ServiceSubscribers.Any(x => x.Id == e.Id))
            {
                view.ServiceSubscribers.First(x => x.Id == e.Id)
                    .LegalName = e.LegalName;
            }
        }
        private void WhenServiceSubscriberDisplayNameChanged(ServiceSubscriberDisplayNameChanged e, ActiveServiceSubscribersView view)
        {
            if ( view.ServiceSubscribers.Any(x => x.Id == e.Id))
            {
                view.ServiceSubscribers.First(x => x.Id == e.Id)
                    .DisplayName = e.DisplayName;
            }
        }
    }
}