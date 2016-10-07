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
using System.Text.RegularExpressions;


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
                address = await client.GetAsync<Address>(link);
            }));
            tasks.Add(Task.Factory.StartNew(async () =>
            {
                HalKit.Models.Response.Link link = new HalKit.Models.Response.Link {HRef = item.Customer.AbsolutePath, IsTemplated = false};
                customer = await client.GetAsync<Customer>(link);
            }));
            tasks.Add(Task.Factory.StartNew(async () =>
            {
                HalKit.Models.Response.Link link = new HalKit.Models.Response.Link {HRef = item.Card.AbsolutePath, IsTemplated = false};
                card = await client.GetAsync<Card>(link);
            }));
            tasks.Add(Task.Factory.StartNew(async () =>
            {
                HalKit.Models.Response.Link link = new HalKit.Models.Response.Link {HRef = item.Items.AbsolutePath, IsTemplated = false};
                items = await client.GetAsync<List<Item>>(link);
            }));

            Task finalTask = Task.Factory.ContinueWhenAll(
                tasks.ToArray(),
                _ =>
                {
                });
            finalTask.Wait();

            float amount = CalculateTotal(items);

            PaymentRequest paymentRequest = new PaymentRequest() {
                Address = address,
                Card = card,
                Customer = customer,
                Amount = amount
            };

            PaymentResponse paymentResponse = null;
            Task PaymentResponseTask = Task.Factory.StartNew(async () =>
            {
                HalKit.Models.Response.Link link = new HalKit.Models.Response.Link {HRef =  "http://payment/paymentAuth", IsTemplated = false};
                paymentResponse = await client.PostAsync<PaymentResponse>(link, paymentRequest);
            });
            PaymentResponseTask.Wait();
            
            if(!paymentResponse.Authorized) {
                return BadRequest();
            }

            string ACustomerId = customer.Id; //ParseId(customer.I)
            Shipment Shipment = null;
            Task ShipmentTask = Task.Factory.StartNew(async () =>
            {
                HalKit.Models.Response.Link link = new HalKit.Models.Response.Link {HRef =  "http://shipping/shipping", IsTemplated = false};
                Shipment AShipment = new Shipment() {
                    Name = ACustomerId
                };
                Shipment = await client.PostAsync<Shipment>(link, AShipment);
            });
            ShipmentTask.Wait();

            CustomerOrder order = new CustomerOrder() {
                CustomerId = ACustomerId,
                Address = address,
                Card  = card,
                Customer = customer,
                Items = items,
                Total = amount,
                Shipment = Shipment
            };
            customerOrderRepository.Add(order);
            return CreatedAtRoute("GetTodo", new { id = order.Id }, order);

        }

        private float CalculateTotal(List<Item> items) {
            float amount = 0F;
            float shipping = 4.99F;
            items.ForEach(item => amount += item.Quantity * item.UnitPrice);
            amount += shipping;
            return amount;
        }
    

        private string ParseId(string href) {
            string pattern = @"[\\w-]+$";

            Regex r = new Regex(pattern);
            Match match = r.Match(href);
            if (match.Success)
            {
                return match.Value;
            }
            throw new System.ArgumentException("No match found", href);
        }
    }
}