using System;
using Ezley.Events;
using Ezley.ValueObjects;
using Ezley.ProjectionStore;

namespace Ezley.Projections
{
    public class ServiceSubscriberView
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string LegalName { get; set; }
        public string DisplayName { get; set; }
        public Address Address { get; set; }
        public Phone Phone { get; set; }
        public Email Email { get; set; }
        public bool Active { get; set; }
    }
    public class ServiceSubscriberProjection: Projection<ServiceSubscriberView>
    {
        public ServiceSubscriberProjection()
        {
            RegisterHandler<ServiceSubscriberRegistered>(WhenServiceSubscriberRegistered);
            RegisterHandler<ServiceSubscriberLegalNameChanged>(WhenServiceSubscriberLegalNameChanged);
            RegisterHandler<ServiceSubscriberDisplayNameChanged>(WhenServiceSubscriberDisplayNameChanged);
            RegisterHandler<ServiceSubscriberAddressChanged>(WhenServiceSubscriberAddressChanged);
            RegisterHandler<ServiceSubscriberPhoneChanged>(WhenServiceSubscriberPhoneChanged);
            RegisterHandler<ServiceSubscriberEmailChanged>(WhenServiceSubscriberEmailChanged);
            RegisterHandler<ServiceSubscriberActivated>(WhenServiceSubscriberActivated);
            RegisterHandler<ServiceSubscriberDeactivated>(WhenServiceSubscriberDeactivated);
        }

        private void WhenServiceSubscriberRegistered(ServiceSubscriberRegistered e, ServiceSubscriberView view)
        {
            view.Id = e.Id;
            view.TenantId = e.TenantId;
            view.LegalName = e.LegalName;
            view.DisplayName = e.DisplayName;
            view.Address = e.Address;
            view.Phone = e.Phone;
            view.Email = e.Email;
            view.Active = e.Active;
        }
        
        private void WhenServiceSubscriberLegalNameChanged(ServiceSubscriberLegalNameChanged e, ServiceSubscriberView view)
        {
            view.LegalName = e.LegalName;
        }
        private void WhenServiceSubscriberDisplayNameChanged(ServiceSubscriberDisplayNameChanged e, ServiceSubscriberView view)
        {
            view.DisplayName = e.DisplayName;
        }
        private void WhenServiceSubscriberAddressChanged(ServiceSubscriberAddressChanged e, ServiceSubscriberView view)
        {
            view.Address = e.Address;
        }
        private void WhenServiceSubscriberPhoneChanged(ServiceSubscriberPhoneChanged e, ServiceSubscriberView view)
        {
            view.Phone = e.Phone;
        }
        private void WhenServiceSubscriberEmailChanged(ServiceSubscriberEmailChanged e, ServiceSubscriberView view)
        {
            view.Email = e.Email;
        }
        private void WhenServiceSubscriberActivated(ServiceSubscriberActivated e, ServiceSubscriberView view)
        {
            view.Active = true;
        }
        private void WhenServiceSubscriberDeactivated(ServiceSubscriberDeactivated e, ServiceSubscriberView view)
        {
            view.Active = false;
        }
    }
}