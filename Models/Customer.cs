using System.Collections.Generic;

namespace CustomerOrdersApi
{
    public class Customer
    {
        private string Id { get; set; }
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string Username { get; set; }
        private List<Address> Addresses { get; set; } = new List<Address>{};
        private List<Card> Cards { get; set; } = new List<Card>{};
    }
}