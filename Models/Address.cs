using HalKit.Models.Response;
using Newtonsoft.Json;

namespace CustomerOrdersApi.Model
{
    public class Address : Entity
    {
        [JsonProperty(PropertyName = "number")]
        private string Number { get; set; }
        [JsonProperty(PropertyName = "street")]
        private string Street { get; set; }
        [JsonProperty(PropertyName = "city")]
        private string City { get; set; }
        [JsonProperty(PropertyName = "postcode")]
        private string Postcode { get; set; }
        [JsonProperty(PropertyName = "country")]
        private string Country { get; set; }
    }
}
