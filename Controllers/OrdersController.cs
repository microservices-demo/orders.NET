using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MongoRepository;


namespace CustomerOrdersApi
{
    [Route("/orders")]
    public class ProductsController
    {
        private MongoRepository<CustomerOrder> customerOrderRepository = new MongoRepository<CustomerOrder>("mongodb://orders-db:27017/data", "CustomerOrder");
        private static List<CustomerOrder> _orders = 
            new List<CustomerOrder>(new[] {
            new CustomerOrder() { Id = "1" },
            new CustomerOrder() { Id = "2" },
            new CustomerOrder() { Id = "3", CustomerId = "3" },
            new CustomerOrder() { Id = "4" },
            new CustomerOrder() { Id = "5" }
        });

        public IEnumerable<CustomerOrder> Get()
        {
            return _orders;
        }
    }
}