namespace Ezley.Testing
{
    public class TestConfig
    {
        public readonly bool UseInMemoryEventStore = false;
        public readonly string EndpointUri = "https://yourdb.documents.azure.com:443/";
        public readonly string Database = "yourdb";

        public readonly string AuthKey = 
            "yourkey==";

        public readonly string EventContainer = "events";
        public readonly string LeasesContainer = "leases";
        public readonly string ViewContainer = "views";
        public readonly string SnapshotContainer = "snapshots";
 
    }
}