using System.Collections.Generic;
using HalKit.Models.Response;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CustomerOrdersApi.Model
{
    public class Address : Entity
    {
        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }
        [JsonProperty(PropertyName = "street")]
        public string Street { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "postcode")]
        public string Postcode { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
    }
}
