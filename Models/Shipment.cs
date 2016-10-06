namespace CustomerOrdersApi
{
    public class Shipment
    {
        public string Id { get; set; } = System.Guid.NewGuid().ToString();
        public string Name { get; set; }
    }
}