using Newtonsoft.Json;
namespace CustomerOrdersApi.Model
{
    public class PaymentResponse
    {
        [JsonProperty(PropertyName = "authorised")]
        public bool Authorised { get; set; } = false;
    }
}
