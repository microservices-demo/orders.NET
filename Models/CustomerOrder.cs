using System.Collections.Generic;
using System;

namespace CustomerOrdersApi
{
    public class CustomerOrder
    {
        public int Id { get; set; }
        private string CustomerId  { get; set; }
        private Customer Customer { get; set; }
        private Address Address { get; set; }
        private Card Card { get; set; }
        private List<Item> Items { get; set; } = new List<Item>{};
        private Shipment Shipment { get; set; }
        private DateTime Date { get;  set; } = DateTime.Now;
        private float Total { get; set; }
    }
}