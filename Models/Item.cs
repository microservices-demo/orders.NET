using HalKit.Models.Response;
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
