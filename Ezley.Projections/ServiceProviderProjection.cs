using System;
using Ezley.Events;
using Ezley.ValueObjects;
using Ezley.ProjectionStore;

namespace Ezley.Projections
{
    public class TenantView
    {
        public Guid Id { get; set; }
        public string LegalName { get; set; }
        public string DisplayName { get; set; }
        public Address Address { get; set; }
        public Phone Phone { get; set; }
        public Email Email { get; set; }
    }
    public class TenantProjection: Projection<TenantView>
    {
        public TenantProjection()
        {
            RegisterHandler<TenantRegistered>(WhenTenantRegistered);
            RegisterHandler<TenantLegalNameChanged>(WhenTenantLegalNameChanged);
            RegisterHandler<TenantDisplayNameChanged>(WhenTenantDisplayNameChanged);
            RegisterHandler<TenantAddressChanged>(WhenTenantAddressChanged);
            RegisterHandler<TenantPhoneChanged>(WhenTenantPhoneChanged);
            RegisterHandler<TenantEmailChanged>(WhenTenantEmailChanged);
        }

        private void WhenTenantRegistered(TenantRegistered e, TenantView view)
        {
            view.Id = e.Id;
            view.LegalName = e.LegalName;
            view.DisplayName = e.DisplayName;
            view.Address = e.Address;
            view.Phone = e.Phone;
            view.Email = e.Email;
        }
        
        private void WhenTenantLegalNameChanged(TenantLegalNameChanged e, TenantView view)
        {
            view.LegalName = e.LegalName;
        }
        private void WhenTenantDisplayNameChanged(TenantDisplayNameChanged e, TenantView view)
        {
            view.DisplayName = e.DisplayName;
        }
        private void WhenTenantAddressChanged(TenantAddressChanged e, TenantView view)
        {
            view.Address = e.Address;
        }
        private void WhenTenantPhoneChanged(TenantPhoneChanged e, TenantView view)
        {
            view.Phone = e.Phone;
        }
        private void WhenTenantEmailChanged(TenantEmailChanged e, TenantView view)
        {
            view.Email = e.Email;
        }
    }
}