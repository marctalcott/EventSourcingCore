using System;

namespace Ezley.API.Commands.ViewModels
{
    public class AddItemToOrderViewModel
    {
        public DateTime TimeAdded { get; }
        public string Name { get; }
        public decimal Amount { get; }

        public AddItemToOrderViewModel(string name, decimal amount)
        {
            TimeAdded = DateTime.UtcNow;
            Name = name;
            Amount = amount;
        }
    }
}