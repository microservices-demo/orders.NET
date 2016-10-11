using System.Collections.Generic;
using HalKit.Models.Response;
using Newtonsoft.Json;

namespace CustomerOrdersApi.Model
{
    public class Customer : Resource
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "addresses")]
        public List<Address> Addresses { get; set; } = new List<Address>{};
        [JsonProperty(PropertyName = "cards")]
        public List<Card> Cards { get; set; } = new List<Card>{};
    }
}
