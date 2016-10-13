using HalKit.Models.Response;
using Newtonsoft.Json;
namespace CustomerOrdersApi.Model
{
    public class Card : Entity
    {
        [JsonProperty(PropertyName = "longNum")]
        private string LongNum { get; set; }
        [JsonProperty(PropertyName = "expires")]
        private string Expires { get; set; }
        [JsonProperty(PropertyName = "ccv")]
        private string CCV { get; set; }
    }
}
