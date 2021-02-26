namespace Ezley.Testing
{
    public partial class TestConfig
    {
        public readonly bool UseInMemoryEventStore = false;
        public readonly string EventsEndpointUri = "https://esdemo2.documents.azure.com:443/";
        public readonly string EventsDatabase = "esdemo";
        public readonly string EventsAuthKey = 
            "soVzlHrvy6wgipZOiDnXOFlyojfXAtyCvCpJgVsT3JpyzFiCIHNtgA5cZ3cV8prZ9iJjv9RFwaWm2Mw9dIvHyw==";
        public readonly string EventContainer = "events";
        
        public readonly string LeasesEndpointUri = "https://esdemo2.documents.azure.com:443/";
        public readonly string LeasesDatabase = "esdemo";
        public readonly string LeasesAuthKey = 
            "soVzlHrvy6wgipZOiDnXOFlyojfXAtyCvCpJgVsT3JpyzFiCIHNtgA5cZ3cV8prZ9iJjv9RFwaWm2Mw9dIvHyw==";
        public readonly string LeasesContainer = "leases";
        
        public readonly string ViewsEndpointUri = "https://esdemo2.documents.azure.com:443/";
        public readonly string ViewsDatabase = "esdemo";
        public readonly string ViewsAuthKey = 
            "soVzlHrvy6wgipZOiDnXOFlyojfXAtyCvCpJgVsT3JpyzFiCIHNtgA5cZ3cV8prZ9iJjv9RFwaWm2Mw9dIvHyw==";
        public readonly string ViewsContainer = "views";
        
        public readonly string SnapshotsEndpointUri = "https://esdemo2.documents.azure.com:443/";
        public readonly string SnapshotsDatabase = "esdemo";
        public readonly string SnapshotsAuthKey = 
            "soVzlHrvy6wgipZOiDnXOFlyojfXAtyCvCpJgVsT3JpyzFiCIHNtgA5cZ3cV8prZ9iJjv9RFwaWm2Mw9dIvHyw==";
        public readonly string SnapshotsContainer = "snapshots";
       
    }
}