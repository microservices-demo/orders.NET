using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Halcyon.HAL.Attributes;
namespace CustomerOrdersApi.Model
{
    [HalModel("~/orders", true)]
    [HalLink("order", "orders/{Id}")]
    [HalLink("self", "orders/{Id}")]
    public class CustomerOrder : Entity
    {
        [JsonProperty(PropertyName = "id")]
        // without this override [HalLink("self", "orders/{Id}")] does not work
        public override string Id { get; set; }
        [JsonProperty(PropertyName = "customerId")]
        public string CustomerId  { get; set; }
        [HalEmbedded("customer")]
        [JsonProperty(PropertyName = "customer")]
        public Customer Customer { get; set; }
        [HalEmbedded("address")]
        [JsonProperty(PropertyName = "address")]
        public Address Address { get; set; }
        [JsonProperty(PropertyName = "card")]
        public Card Card { get; set; }
        [HalEmbedded("items")]
        [JsonProperty(PropertyName = "items")]
        public List<Item> Items { get; set; } = new List<Item>{};
        [HalEmbedded("shipment")]
        [JsonProperty(PropertyName = "shipment")]
        public Shipment Shipment { get; set; }
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get;  set; } = DateTime.Now;
        [JsonProperty(PropertyName = "total")]
        public float Total { get; set; }
    }
}
