using System;

namespace Ezley.Testing
{
    public partial class TestConfig
    {
        // monitor changefeed startTime Exclusive (start after this).
        public long StartTimeEpoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        
        public readonly bool UseInMemoryEventStore = false;
        public readonly string EventsEndpointUri = "https://esdemo2.documents.azure.com:443/";
        public readonly string EventsDatabase = "esdemo";
        public readonly string EventsAuthKey = 
            "CMQjeSQZ9QDwaQT7bSkAX1ZbHfyHy69qWfC1cCThsqUEC8hpZKclQuCIxUSWZzVObCsVmDeElu8k97opO38gpw==";
        public readonly string EventContainer = "events";
        
        public readonly string LeasesEndpointUri = "https://esdemo2.documents.azure.com:443/";
        public readonly string LeasesDatabase = "esdemo";
        public readonly string LeasesAuthKey = 
            "CMQjeSQZ9QDwaQT7bSkAX1ZbHfyHy69qWfC1cCThsqUEC8hpZKclQuCIxUSWZzVObCsVmDeElu8k97opO38gpw==";
        public readonly string LeasesContainer = "leases";
        
        public readonly string OrderViewsEndpointUri = "https://esdemo2.documents.azure.com:443/";
        public readonly string OrderViewsDatabase = "esdemo";
        public readonly string OrderViewsAuthKey = 
            "CMQjeSQZ9QDwaQT7bSkAX1ZbHfyHy69qWfC1cCThsqUEC8hpZKclQuCIxUSWZzVObCsVmDeElu8k97opO38gpw==";
        public readonly string OrderViewsContainer = "orderviews";
        
        public readonly string CustomerViewsEndpointUri = "https://esdemo2.documents.azure.com:443/";
        public readonly string CustomerViewsDatabase = "esdemo";
        public readonly string CustomerViewsAuthKey = 
            "CMQjeSQZ9QDwaQT7bSkAX1ZbHfyHy69qWfC1cCThsqUEC8hpZKclQuCIxUSWZzVObCsVmDeElu8k97opO38gpw==";
        public readonly string CustomerViewsContainer = "customerviews";
        
        public readonly string SnapshotsEndpointUri = "https://esdemo2.documents.azure.com:443/";
        public readonly string SnapshotsDatabase = "esdemo";
        public readonly string SnapshotsAuthKey = 
            "CMQjeSQZ9QDwaQT7bSkAX1ZbHfyHy69qWfC1cCThsqUEC8hpZKclQuCIxUSWZzVObCsVmDeElu8k97opO38gpw==";
        public readonly string SnapshotsContainer = "snapshots";
        
        
       
    }
}