namespace Ezley.Testing
{
    public partial class TestConfig
    {
        public readonly bool xUseInMemoryEventStore = false;
        public readonly string xEndpointUri = "https://yourdb.documents.azure.com:443/";
        public readonly string xDatabase = "yourdb";

        public readonly string xAuthKey = 
            "yourkey==";

        public readonly string xEventContainer = "events";
        public readonly string xLeasesContainer = "leases";
        public readonly string xViewContainer = "views";
        public readonly string xSnapshotContainer = "snapshots";
 
    }
}