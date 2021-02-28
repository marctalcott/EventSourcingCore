using System;

namespace Ezley.Testing
{
    public partial class TestConfig
    {
        // monitor changefeed startTime Exclusive (start after this).
        public long StartTimeEpoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        
        public readonly bool UseInMemoryEventStore = false;
        public readonly string EventsEndpointUri = "https://your db.documents.azure.com:443/";
        public readonly string EventsDatabase = "esdemo";
        public readonly string EventsAuthKey = 
            "your key ==";
        public readonly string EventContainer = "events";
        
        public readonly string LeasesEndpointUri = "https://your db.documents.azure.com:443/";
        public readonly string LeasesDatabase = "esdemo";
        public readonly string LeasesAuthKey = 
            "your key ==";
        public readonly string LeasesContainer = "leases";
        
        public readonly string OrderViewsEndpointUri = "https://your db.documents.azure.com:443/";
        public readonly string OrderViewsDatabase = "esdemo";
        public readonly string OrderViewsAuthKey = 
            "your key==";
        public readonly string OrderViewsContainer = "orderviews";
        
        public readonly string CustomerViewsEndpointUri = "https://your db.documents.azure.com:443/";
        public readonly string CustomerViewsDatabase = "esdemo";
        public readonly string CustomerViewsAuthKey = 
            "your key==";
        public readonly string CustomerViewsContainer = "customerviews";
        
        public readonly string SnapshotsEndpointUri = "https://your db.documents.azure.com:443/";
        public readonly string SnapshotsDatabase = "esdemo";
        public readonly string SnapshotsAuthKey = 
            "your key==";
        public readonly string SnapshotsContainer = "snapshots";
        
        
       
    }
}