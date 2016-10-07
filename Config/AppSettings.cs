using System;
namespace CustomerOrdersApi
{
    public class AppSettings
    {
        public BaseUrls BaseUrls { get; set; }
    }
    public class BaseUrls
    {
        public Uri Payment { get; set; }
        public Uri Shipping { get; set; }
    }   
}