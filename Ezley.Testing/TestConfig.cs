using System;

namespace Ezley.Testing
{
    public partial class TestConfig
    {
        // monitor changefeed startTime Exclusive (start after this).
        public long StartTimeEpoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        
        public readonly bool UseInMemoryEventStore = false;
        public readonly string EventsEndpointUri = "https://[yourdb].documents.azure.com:443/";
        public readonly string EventsDatabase = "esdemo";
        public readonly string EventsAuthKey = 
            "[yourkey]";
        public readonly string EventContainer = "events";
        
        public readonly string LeasesEndpointUri = "https://[yourdb].documents.azure.com:443/";
        public readonly string LeasesDatabase = "esdemo";
        public readonly string LeasesAuthKey = 
            "[yourkey]";
        public readonly string LeasesContainer = "leases";
        
        public readonly string OrderViewsEndpointUri = "https://[yourdb].documents.azure.com:443/";
        public readonly string OrderViewsDatabase = "esdemo";
        public readonly string OrderViewsAuthKey = 
            "[yourkey]";
        public readonly string OrderViewsContainer = "orderviews";
        
        public readonly string CustomerViewsEndpointUri = "https://[yourdb].documents.azure.com:443/";
        public readonly string CustomerViewsDatabase = "esdemo";
        public readonly string CustomerViewsAuthKey = 
            "[yourkey]";
        public readonly string CustomerViewsContainer = "customerviews";
        
        public readonly string SnapshotsEndpointUri = "https://[yourdb].documents.azure.com:443/";
        public readonly string SnapshotsDatabase = "esdemo";
        public readonly string SnapshotsAuthKey = 
             "[yourkey]";
        public readonly string SnapshotsContainer = "snapshots";
        
        
       
    }
}