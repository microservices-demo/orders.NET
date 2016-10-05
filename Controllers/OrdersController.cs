using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using MongoRepository;
using Halcyon.HAL;
using Halcyon.HAL.Attributes;
using Halcyon.Web.HAL;
using CustomerOrdersApi.Model;
using System.Net;
using Newtonsoft.Json;
using HalKit;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using HalKit.Models.Response;
using System;
using System.Threading;

namespace CustomerOrdersApi
{
    [HalModel("~/", true)]
    [HalLink("self", "/orders")]
    [HalLink("profile", "profile/orders")]
    [HalLink("search", "orders/search")]
    public class OrdersModel
    {
        [JsonProperty("page")]
        public ResultPage Page { get; set; } = new ResultPage();
        [JsonIgnore]
        [HalEmbedded("customerOrders")]
        public List<CustomerOrder> Orders { get; set; } = new List<CustomerOrder>();
        public class ResultPage
        {
            [JsonProperty("size")]
            public int Size { get; set; } = 20;
            [JsonProperty("totalElements")]
            public int TotalElements { get; set; }
            [JsonProperty("totalPages")]
            public int TotalPages { get; set; }
            [JsonProperty("number")]
            public int Number { get; set; }
        }
    }

   [Route("/orders")]
   public class ProductsController: Controller
    {
        private HalClient client = new HalClient(new HalConfiguration());
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
            return this.HAL(converter.Convert(model),  HttpStatusCode.OK);
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public IActionResult Create([FromBody] NewOrderResource item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            List<Task> tasks = new List<Task>();
            Address address = null;
            Customer customer = null;
            Card card = null;
            List<Item> items = null;

            tasks.Add(Task.Factory.StartNew(async () =>
            {
                HalKit.Models.Response.Link link = new HalKit.Models.Response.Link {HRef = item.Address.AbsolutePath, IsTemplated = false};
                address = await client.GetAsync<Address>(link);;
            }));
            tasks.Add(Task.Factory.StartNew(async () =>
            {
                HalKit.Models.Response.Link link = new HalKit.Models.Response.Link {HRef = item.Customer.AbsolutePath, IsTemplated = false};
                customer = await client.GetAsync<Customer>(link);;
            }));
            tasks.Add(Task.Factory.StartNew(async () =>
            {
                HalKit.Models.Response.Link link = new HalKit.Models.Response.Link {HRef = item.Card.AbsolutePath, IsTemplated = false};
                card = await client.GetAsync<Card>(link);;
            }));
            tasks.Add(Task.Factory.StartNew(async () =>
            {
                HalKit.Models.Response.Link link = new HalKit.Models.Response.Link {HRef = item.Items.AbsolutePath, IsTemplated = false};
                items = await client.GetAsync<List<Item>>(link);;
            }));

            Task finalTask = Task.Factory.ContinueWhenAll(
                tasks.ToArray(),
                _ =>
                {
                });
            finalTask.Wait();

            float amount = calculateTotal(items);

            CustomerOrder order = new CustomerOrder();
            order.Address = address;
            order.Card  = card;
            order.Customer = customer;
            order.Items = items;
            
            // return result.Unwrap();
            // TodoItems.Add(item);
            return CreatedAtRoute("GetTodo", new { id = order.Id }, order);
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

        private float calculateTotal(List<Item> items) {
            float amount = 0F;
            float shipping = 4.99F;
            items.ForEach(item => amount += item.Quantity * item.UnitPrice);
            amount += shipping;
            return amount;
        }
    }

    
}