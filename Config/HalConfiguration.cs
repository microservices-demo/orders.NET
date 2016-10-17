using HalKit;

namespace CustomerOrdersApi
    {
    public class HalConfiguration : HalKitConfiguration
    {
        public HalConfiguration() : base("http://whatever.com")
        {
        }
    }
}