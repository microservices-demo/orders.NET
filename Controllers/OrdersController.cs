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
            new MongoRepository<CustomerOrder>("mongodb://orders-db:27017/data", "CustomerOrder");
        
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
        [HttpGet("{id}", Name = "GetOffer")]
        public IActionResult Get(string id)
        {
            CustomerOrder order = customerOrderRepository.GetById(id);
            if(order == null) {
                    return NotFound();
            }
            return new ObjectResult(order);
        }

        [HttpPost]
        public IActionResult Create([FromBody] NewOrderResource item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            Console.WriteLine("item:" + ToStringNullSafe(JsonConvert.SerializeObject(item)));

           Address address = null;
            Customer customer = null;
            Card card = null;
            List<Item> items = null;

           Task.Factory.ContinueWhenAll(
               new Task[] {
                   client.GetAsync<Address>(new HalKit.Models.Response.Link {HRef = item.Address.AbsoluteUri, IsTemplated = false})
                        .ContinueWith((task) => {
					Console.WriteLine("returning address:" + ToStringNullSafe(JsonConvert.SerializeObject(task.Result)));
					address = task.Result;
					}),
                   client.GetAsync<Customer>(new HalKit.Models.Response.Link {HRef = item.Customer.AbsoluteUri, IsTemplated = false})
                        .ContinueWith((task) => {
					Console.WriteLine("returning customer:" + ToStringNullSafe(JsonConvert.SerializeObject(task.Result)));
					customer = task.Result; }),
                   client.GetAsync<Card>(new HalKit.Models.Response.Link {HRef = item.Card.AbsoluteUri, IsTemplated = false})
                        .ContinueWith((task) => {
					Console.WriteLine("returning card:" + ToStringNullSafe(JsonConvert.SerializeObject(task.Result)));
					card = task.Result; }),
			Task.Factory.StartNew( () =>
					{
					try{
					client.GetAsync<List<Item>>(new HalKit.Models.Response.Link {HRef = item.Items.AbsoluteUri, IsTemplated = false}
							, new Dictionary<string, string> ()
							, new Dictionary<string, IEnumerable<string>>
							{
							  {"Accept", new[] {"application/json", "application/hal+json"}}

							})
					.ContinueWith((task) => {
							Console.WriteLine("returning items from " + item.Items.AbsoluteUri + " :" + ToStringNullSafe(JsonConvert.SerializeObject(task.Result)));
							items = task.Result; }).Wait();
					} catch (Exception e) {
						Console.WriteLine("Exception : " + e.ToString()); 
					}
					})
	       },
                _ => {})
                    .Wait();

            PaymentResponse paymentResponse = null;

            float amount = CalculateTotal(items);
            PaymentRequest paymentRequest = new PaymentRequest() {
                Address = address,
                Card = card,
                Customer = customer,
                Amount = amount
            };

            client.PostAsync<PaymentResponse>(new HalKit.Models.Response.Link {HRef =  "http://payment/paymentAuth", IsTemplated = false}, paymentRequest)
                    .ContinueWith((task) => { 
                                    paymentResponse = task.Result; 
                                    Console.WriteLine("returning:" + ToStringNullSafe(JsonConvert.SerializeObject(paymentResponse)));
})               .Wait();
            
            if(!paymentResponse.Authorised) {
                return BadRequest();
            }

            string ACustomerId = customer.Id;
            Shipment Shipment = null;
                Shipment AShipment = new Shipment() {
                    Name = ACustomerId
                };
                client.PostAsync<Shipment>(new HalKit.Models.Response.Link {HRef =  "http://shipping/shipping", IsTemplated = false}, AShipment)
                        .ContinueWith((task) => {
                                Shipment = task.Result;
                        })
                .Wait();
            Console.WriteLine("Shipment:" + JsonConvert.SerializeObject(Shipment));

            customer.Id = null;
            address.Id = null;
            card.Id = null;

            CustomerOrder order = new CustomerOrder() {
                CustomerId = ACustomerId,
                Address = address,
                Card  = card,
                Customer = customer,
                Items = items,
                Total = amount,
                Shipment = Shipment
            };

            Console.WriteLine("Order:" + JsonConvert.SerializeObject(order));
            customerOrderRepository.Add(order);
            Console.WriteLine("Order2:" + JsonConvert.SerializeObject(order));
            //return CreatedAtRoute("GetOffer", new { id = order.Id }, order);
            return new ObjectResult(order) {
                StatusCode = 201
            };
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

        private string ToStringNullSafe(object value) {
            return (value ?? string.Empty).ToString();
        }
    }
}
