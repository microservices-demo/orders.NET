using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using MongoRepository;
using Halcyon.HAL;
using Halcyon.HAL.Attributes;
using Halcyon.Web.HAL;
using CustomerOrdersApi.Model;

namespace CustomerOrdersApi
{
    [HalModel("~/orders", true)]
    [HalLink("self", "/orders")]
    [HalLink("profile", "profile/orders")]
    [HalLink("search", "orders/search")]
    public class OrdersModel
    {
        [HalEmbedded("customerOrders")]
        public List<CustomerOrder> Orders { get; set; } = new List<CustomerOrder>();
    }

   [Route("/orders")]
   public class ProductsController: Controller
    {

        private HALAttributeConverter converter = new HALAttributeConverter();
        private MongoRepository<CustomerOrder> customerOrderRepository = 
            new MongoRepository<CustomerOrder>("mongodb://localhost:27017/data", "CustomerOrder");
        
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerator<CustomerOrder> enumerator = customerOrderRepository.GetEnumerator();
            OrdersModel model = new OrdersModel();
            while (enumerator.MoveNext())
            {
                model.Orders.Add(enumerator.Current);
            }

            return this.HAL(model, new Link[] {
                new Link("self", "/orders"),
                new Link("profile", "profile/orders"),
                new Link("search", "orders/search")
            });
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public IActionResult Create([FromBody] CustomerOrder item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            // List<Task> tasks = new List<Task>();
            // tasks.Add(Task.Factory.StartNew(o =>
            //     {
            //         int value = (int)o;
            //     }, i);


            // var result = Task.Factory.ContinueWhenAll(
            //     new[] { getPlatformTask, getUserTask },
            //     _ =>
            //     {
            //         // process the results

            //         return base.SendAsync(request, cancellationToken);
            //     });

            // return result.Unwrap();
            // TodoItems.Add(item);
            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] CustomerOrder item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            // var todo = TodoItems.Find(id);
            // if (todo == null)
            // {
            //     return NotFound();
            // }

            // TodoItems.Update(item);
            return new NoContentResult();
        }
    }

    
}