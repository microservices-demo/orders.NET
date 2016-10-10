using HalKit.Models.Response;
using Newtonsoft.Json;
namespace CustomerOrdersApi.Model
{
    public class Card : Resource
    {
        [JsonProperty(PropertyName = "id")]
        private string Id { get; set; }
        [JsonProperty(PropertyName = "longnum")]
        private string LongNum { get; set; }
        [JsonProperty(PropertyName = "expires")]
        private string Expires { get; set; }
        [JsonProperty(PropertyName = "ccv")]
        private string CCV { get; set; }
    }
}