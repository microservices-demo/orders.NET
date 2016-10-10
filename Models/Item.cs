using HalKit.Models.Response;
using Newtonsoft.Json;

namespace CustomerOrdersApi.Model
{
    public class Item : Resource
    {   
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "itemid")]
        public string ItemId { get; set; }
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }
        [JsonProperty(PropertyName = "unitprice")]
        public float UnitPrice { get; set; }
    }
}