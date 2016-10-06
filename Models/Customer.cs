using System.Collections.Generic;

namespace CustomerOrdersApi.Model
{
    public class Customer
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>{};
        public List<Card> Cards { get; set; } = new List<Card>{};
    }
}