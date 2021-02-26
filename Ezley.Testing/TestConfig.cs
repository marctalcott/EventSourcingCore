using System;

namespace Ezley.Testing
{
    public partial class TestConfig
    {
        // monitor changefeed startTime Exclusive (start after this).
        public long StartTimeEpoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        
        public readonly bool UseInMemoryEventStore = false;
        public readonly string EventsEndpointUri = "";
        public readonly string EventsDatabase = "esdemo";
        public readonly string EventsAuthKey = 
            "";
        public readonly string EventContainer = "events";
        
        public readonly string LeasesEndpointUri = "";
        public readonly string LeasesDatabase = "esdemo";
        public readonly string LeasesAuthKey = 
            "";
        public readonly string LeasesContainer = "leases";
        
        public readonly string ViewsEndpointUri = "";
        public readonly string ViewsDatabase = "esdemo";
        public readonly string ViewsAuthKey = 
            "";
        public readonly string ViewsContainer = "views";
        
        public readonly string SnapshotsEndpointUri = "";
        public readonly string SnapshotsDatabase = "esdemo";
        public readonly string SnapshotsAuthKey = 
            "";
        public readonly string SnapshotsContainer = "snapshots";
        
        
       
    }
}