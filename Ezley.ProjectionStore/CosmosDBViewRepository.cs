using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Ezley.ProjectionStore
{
    public class CosmosDBViewRepository : IViewRepository
    {
        private readonly CosmosClient _client;
        private readonly string _databaseId;
        private readonly string _containerId;

        public CosmosDBViewRepository(string endpointUrl, string authorizationKey, string databaseId,
            string containerId)
        {
            _client = new CosmosClient(endpointUrl, authorizationKey);
            _databaseId = databaseId;
            _containerId = containerId;
        }

        public async Task<View> LoadViewAsync(string name)
        {
            var container = _client.GetContainer(_databaseId, _containerId);
            var partitionKey = new PartitionKey(name);

            try
            {
                var response = await container.ReadItemAsync<View>(name, partitionKey);
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return new View();
            }
        }
        
        public async Task<T> LoadTypedViewAsync<T>(string name)
        {
            var view = await LoadViewAsync(name);
            var item = view.Payload.ToObject<T>();
            return item;
        }

        public async Task<bool> SaveViewAsync(string name, View view)
        {
            var container = _client.GetContainer(_databaseId, _containerId);
            var partitionKey = new PartitionKey(name);

            var item = new 
            {
                id = name,
                logicalCheckpoint = view.LogicalCheckpoint,
                payload = view.Payload
            };

            try
            {
                await container.UpsertItemAsync(item, partitionKey, new ItemRequestOptions
                {
                    IfMatchEtag = view.Etag
                });

                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                return false;
            }
        }
    }
}