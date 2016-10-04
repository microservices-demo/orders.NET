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
        private Customer Customer { get; set; }
        private Address Address { get; set; }
        private Card Card { get; set; }
        [HalEmbedded("items")]
        private List<Item> Items { get; set; } = new List<Item>{};
        [JsonIgnore]
        private Shipment Shipment { get; set; }
        [JsonIgnore]
        private DateTime Date { get;  set; } = DateTime.Now;
        [JsonIgnore]
        private float Total { get; set; }
    }
}