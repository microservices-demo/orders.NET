namespace CustomerOrdersApi.Model
{
    public class Item
    {   
        public string Id { get; set; }
        public string ItemId { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
    }
}