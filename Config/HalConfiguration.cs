using HalKit;

namespace CustomerOrdersApi
    {
    public class HalConfiguration : HalKitConfiguration
    {
        public HalConfiguration() : base("http://localhost:5000") // todo put this into config
        {
        }
    }
}