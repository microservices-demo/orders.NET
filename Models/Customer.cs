using System.Collections.Generic;
using HalKit.Models.Response;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CustomerOrdersApi.Model
{
    public class Customer : Entity
    {
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
