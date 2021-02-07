using System;
using System.Collections.Generic;
using Ezley.ValueObjects;
using Newtonsoft.Json;

namespace Ezley.OrderSystem
{
    public class OrderSnapshot
    {
        [JsonProperty] 
        public Guid Id { get; }
        [JsonProperty] 
        public string OrderName { get; }
        [JsonProperty] 
        public List<OrderItem> Items { get; }

        [JsonProperty] 
        public int Version { get; }
        public OrderSnapshot(Guid id, string orderName, List<OrderItem> items, int version)
        {
            Id = id;
            OrderName = orderName;
            Items = items;
            Version = version;
        }
    }
}