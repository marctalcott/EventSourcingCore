using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ezley.ProjectionStore
{
    public class ViewCheckpoint
    {
        public ViewCheckpoint()
        {
            LogicalSequenceNumber = -1;
            ItemIds = new List<string>();
        }

        [JsonProperty("lsn")]
        public long LogicalSequenceNumber { get; set; }
        
        [JsonProperty("_ts")]
        public long TimeStamp { get; set; }

        [JsonProperty("itemIds")]
        public List<string> ItemIds { get; }
    }
}