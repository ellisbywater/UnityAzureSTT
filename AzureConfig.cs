using System;

namespace Reader
{
    [Serializable]
    public class AzureConfig
    {
        public string SubscriptionKey {get; set; }
        public string Region {get; set; }
    }
}
