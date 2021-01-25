namespace Ezley.Testing
{
    public class TestConfig
    {
        public readonly string EndpointUri = "https://cosmosesdemo.documents.azure.com:443/";
        public readonly string Database = "propmanagerevents";

        public readonly string AuthKey = 
            "Dtmt5VK9KXfockzByk2JGgQiG4NU7bmCKD6yoY3adJattQuj4hPY0XWAuRs1gbsVB6psHbmda7nh1iFjQWLrMA==";

        public readonly string EventContainer = "testevents";
        public readonly string LeasesContainer = "testleases";
        public readonly string ViewContainer = "testviews";
        public readonly string SnapshotContainer = "testsnapshots";

    }
}