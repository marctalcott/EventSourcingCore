using Newtonsoft.Json;

namespace Ezley.EventSourcing
{
    public class StreamInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }
    }
}