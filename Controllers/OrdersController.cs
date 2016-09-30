using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MongoRepository;

namespace CustomerOrdersApi
{
    [Route("/orders")]
    public class ProductsController: ControllerBase
    {
        private MongoRepository<CustomerOrder> customerOrderRepository = 
            new MongoRepository<CustomerOrder>("mongodb://localhost:27017/data", "CustomerOrder");
        
        [HttpGet]
        public IEnumerable<CustomerOrder> Get()
        {
            IEnumerator<CustomerOrder> enumerator = customerOrderRepository.GetEnumerator();
            List<CustomerOrder> result = new List<CustomerOrder>();
            while (enumerator.MoveNext())
            {
                result.Add(enumerator.Current);
                // Perform logic on the item
            }
            return result;
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