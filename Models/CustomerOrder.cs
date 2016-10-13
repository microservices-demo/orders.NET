using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Halcyon.HAL.Attributes;
namespace CustomerOrdersApi.Model
{
    public class CustomerOrder : Entity
    {
        [JsonProperty(PropertyName = "customerId")]
        public string CustomerId  { get; set; }
        [JsonProperty(PropertyName = "customer")]
        public Customer Customer { get; set; }
        [JsonProperty(PropertyName = "address")]
        public Address Address { get; set; }
        [JsonProperty(PropertyName = "card")]
        public Card Card { get; set; }
        [HalEmbedded("items")]
        [JsonProperty(PropertyName = "items")]
        public List<Item> Items { get; set; } = new List<Item>{};
        [JsonProperty(PropertyName = "shipment")]
        public Shipment Shipment { get; set; }
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get;  set; } = DateTime.Now;
        [JsonProperty(PropertyName = "total")]
        public float Total { get; set; }
    }
}
