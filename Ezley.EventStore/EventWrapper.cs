using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ezley.EventSourcing
{
    public class EventWrapper
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("stream")]
        public StreamInfo StreamInfo { get; set; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("eventData")]
        public JObject EventData { get; set; }
        
        [JsonProperty("userInfo")]
        public JObject UserInfo { get; set; }

        public IEvent GetEvent(IEventTypeResolver eventTypeResolver)
        { 
            Type eventType = eventTypeResolver.GetEventType(EventType);
            return (IEvent)EventData.ToObject(eventType);
        }
    }
}