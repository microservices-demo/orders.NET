namespace CustomerOrdersApi.Config
{
    public class AppSettings
    {
        public ServiceEndpoints ServiceEndpoints { get; set; }
        public DataSettings Data { get; set; }
    }

    public class ServiceEndpoints {
        public string PaymentServiceEndpoint {get; set; }
        public string ShippingServiceEndpoint {get; set; }
    }

    public class DataSettings {
        public MongoConnection MongoConnection {get; set; }
    }

    public class MongoConnection {
        public string ConnectionString {get; set; }
        public string Database {get; set; }
    }
}