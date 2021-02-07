using System;
using System.Collections.Generic;
using Ezley.ValueObjects;

namespace Ezley.API.Commands.ViewModels
{
    public class PlaceOrderViewModel
    {
        public Guid Id { get; set; }
        public string OrderName { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}