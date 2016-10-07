namespace CustomerOrdersApi.Model
{
    public class PaymentRequest
    {
        public Address Address { get; set; }
        public Card Card { get; set; }
        public Customer Customer { get; set; }
        public float Amount { get; set; }
    }
}