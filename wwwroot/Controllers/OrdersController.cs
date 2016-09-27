using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOrdersApi
{
    [Route("/orders")]
    public class ProductsController
    {
        private static List<CustomerOrder> _orders = 
            new List<CustomerOrder>(new[] {
            new CustomerOrder() { Id = 1 },
            new CustomerOrder() { Id = 2 },
            new CustomerOrder() { Id = 3 },
            new CustomerOrder() { Id = 4 },
            new CustomerOrder() { Id = 5 },
            new CustomerOrder() { Id = 6 },
            new CustomerOrder() { Id = 7 },
            new CustomerOrder() { Id = 8 },
            new CustomerOrder() { Id = 9 },
            new CustomerOrder() { Id = 10 },
            new CustomerOrder() { Id = 11 },
            new CustomerOrder() { Id = 12 },
        });

        public IEnumerable<CustomerOrder> Get()
        {
            return _orders;
        }
    }
}