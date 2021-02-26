using Ezley.EventSourcing;
using Newtonsoft.Json;

namespace Ezley.ProjectionStore
{
    public class Change : EventWrapper
    {
        [JsonProperty("_lsn")]
        public long LogicalSequenceNumber { get; set; }
        
        [JsonProperty("_ts")]
        public long TimeStamp { get; set; }
    }
}