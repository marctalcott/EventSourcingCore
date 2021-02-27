using System;

namespace Ezley.Testing
{
    public partial class TestConfig
    {
        // monitor changefeed startTime Exclusive (start after this).
        public long StartTimeEpoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        
        public readonly bool UseInMemoryEventStore = false;
        public readonly string EventsEndpointUri = "https://esdemo1.documents.azure.com:443/";
        public readonly string EventsDatabase = "esdemo";
        public readonly string EventsAuthKey = 
            "aENKTvufKynrTcKWbVJH8iMIOR7KIfeh6YqFzN7epC5y3Ad0vVsHNp3UtcSELVL77UCfsTfdxlA12g1ola5PMA==";
        public readonly string EventContainer = "events";
        
        public readonly string LeasesEndpointUri = "https://esdemo1.documents.azure.com:443/";
        public readonly string LeasesDatabase = "esdemo";
        public readonly string LeasesAuthKey = 
            "aENKTvufKynrTcKWbVJH8iMIOR7KIfeh6YqFzN7epC5y3Ad0vVsHNp3UtcSELVL77UCfsTfdxlA12g1ola5PMA==";
        public readonly string LeasesContainer = "leases";
        
        public readonly string ViewsEndpointUri = "https://esdemo1.documents.azure.com:443/";
        public readonly string ViewsDatabase = "esdemo";
        public readonly string ViewsAuthKey = 
            "aENKTvufKynrTcKWbVJH8iMIOR7KIfeh6YqFzN7epC5y3Ad0vVsHNp3UtcSELVL77UCfsTfdxlA12g1ola5PMA==";
        public readonly string ViewsContainer = "views";
        
        public readonly string SnapshotsEndpointUri = "https://esdemo1.documents.azure.com:443/";
        public readonly string SnapshotsDatabase = "esdemo";
        public readonly string SnapshotsAuthKey = 
            "aENKTvufKynrTcKWbVJH8iMIOR7KIfeh6YqFzN7epC5y3Ad0vVsHNp3UtcSELVL77UCfsTfdxlA12g1ola5PMA==";
        public readonly string SnapshotsContainer = "snapshots";
        
        
       
    }
}