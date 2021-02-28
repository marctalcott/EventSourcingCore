using System;
using Ezley.Events;
using Ezley.ProjectionStore;

namespace Ezley.Projections
{
    public class CustomerView
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        
        public string MiddleName { get; set; }

        public CustomerView()
        {
        }

        public CustomerView(Guid id, string firstName, string lastName, string middleInitial,
            string middleName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            MiddleInitial = middleInitial;
            MiddleName = middleName;
        }
    }
    
    public class CustomerProjection : Projection<CustomerView>
    {
        public CustomerProjection()
        {
            RegisterHandler<CustomerRegistered>(WhenRegistered);
            RegisterHandler<CustomerFirstNameChanged>(WhenFirstNameChanged);
            RegisterHandler<CustomerLastNameChanged>(WhenLastNameChanged);
            RegisterHandler<CustomerMiddleInitialChanged>(WhenMiddleInitialChanged);
            RegisterHandler<CustomerMiddleNameChanged>(WhenMiddleNameChanged);
        }
        
        private void WhenRegistered(CustomerRegistered e, CustomerView view)
        {
            view.Id = e.Id;
            view.FirstName = e.FirstName;
            view.LastName = e.LastName;
            view.MiddleInitial = e.MiddleInitial;
            view.MiddleName = e.MiddleName;
        }

        private void WhenFirstNameChanged(CustomerFirstNameChanged e, CustomerView view)
        {
            view.FirstName = e.FirstName;
        }
        
        private void WhenLastNameChanged(CustomerLastNameChanged e, CustomerView view)
        {
            view.LastName = e.LastName;
        }
        
        private void WhenMiddleInitialChanged(CustomerMiddleInitialChanged e, CustomerView view)
        {
            view.MiddleInitial = e.MiddleInitial;
        }
        
        private void WhenMiddleNameChanged(CustomerMiddleNameChanged e, CustomerView view)
        {
            view.MiddleName = e.MiddleName;
        }
    }    
}