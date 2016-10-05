using System.Collections.Generic;
using System;
using MongoRepository;
using Newtonsoft.Json;
using Halcyon.HAL.Attributes;
namespace CustomerOrdersApi.Model
{
    [CollectionName("CustomerOrder")]

    [HalModel("~/order", true)]
    [HalLink("self", "order/{ID}")]
    public class CustomerOrder : Entity
    {
        [JsonIgnore]
        public string CustomerId  { get; set; }
        public Customer Customer { get; set; }
        public Address Address { get; set; }
        public Card Card { get; set; }
        [HalEmbedded("items")]
        public List<Item> Items { get; set; } = new List<Item>{};
        [JsonIgnore]
        public Shipment Shipment { get; set; }
        [JsonIgnore]
        public DateTime Date { get;  set; } = DateTime.Now;
        [JsonIgnore]
        public float Total { get; set; }
    }
}