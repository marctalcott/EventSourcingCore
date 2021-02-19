using System;

namespace Ezley.API.Commands.ViewModels
{
    public class RemoveItemFromOrderViewModel
    {
        public string Name { get; }

        public RemoveItemFromOrderViewModel(string name)
        {
            Name = name;
        }
    }
}