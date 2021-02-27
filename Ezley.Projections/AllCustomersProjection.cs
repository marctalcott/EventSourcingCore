using System;
using System.Collections.Generic;
using System.Linq;
using Ezley.Events;
using Ezley.EventSourcing;
using Ezley.ProjectionStore;

namespace Ezley.Projections
{
    public class AllCustomersView
    {
        public List<CustomerView> Customers { get; set; }

        public AllCustomersView()
        {
            Customers = new List<CustomerView>();
        }
        
    }
    
    public class AllCustomersProjection : Projection<AllCustomersView>
    {
        public override string[] GetViewNames(string streamId, IEvent @event) =>
            new string[] { "AllCustomersView"};
        
        public AllCustomersProjection()
        {
            RegisterHandler<CustomerRegistered>(WhenCustomerRegistered);
            RegisterHandler<CustomerFirstNameChanged>(WhenCustomerFirstNameChanged);
            RegisterHandler<CustomerLastNameChanged>(WhenCustomerLastNameChanged);
            RegisterHandler<CustomerMiddleInitialChanged>(WhenCustomerMiddleInitialChanged);
        }
        
        private void WhenCustomerRegistered(CustomerRegistered e, AllCustomersView view)
        {
            var customer = new CustomerView(e.Id, e.FirstName, e.LastName, e.MiddleInitial);
            view.Customers.Add(customer);
        }

        private void WhenCustomerFirstNameChanged(CustomerFirstNameChanged e, AllCustomersView view)
        {
            var existingCustomer = view.Customers.SingleOrDefault(x => x.Id == e.Id);
            if (existingCustomer == null)
            {
                throw new ApplicationException("Customer does not exist...");
            }

            existingCustomer.FirstName = e.FirstName;
        }
        
        private void WhenCustomerLastNameChanged(CustomerLastNameChanged e, AllCustomersView view)
        {
            var existingCustomer = view.Customers.SingleOrDefault(x => x.Id == e.Id);
            if (existingCustomer == null)
            {
                throw new ApplicationException("Customer does not exist...");
            }

            existingCustomer.LastName = e.LastName;
        }

        private void WhenCustomerMiddleInitialChanged(CustomerMiddleInitialChanged e, AllCustomersView view)
        {
            var existingCustomer = view.Customers.SingleOrDefault(x => x.Id == e.Id);
            if (existingCustomer == null)
            {
                throw new ApplicationException("Customer does not exist...");
            }

            existingCustomer.MiddleInitial = e.MiddleInitial;
        }
    }    
}