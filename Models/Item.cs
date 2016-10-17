using HalKit.Models.Response;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CustomerOrdersApi.Model
{
    public class Item : Entity
    {   
        [JsonProperty(PropertyName = "itemId")]
        public string ItemId { get; set; }
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }
        [JsonProperty(PropertyName = "unitPrice")]
        public float UnitPrice { get; set; }
    }
}
