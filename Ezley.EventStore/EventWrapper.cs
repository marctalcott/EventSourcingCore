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
            try
            {
                Type eventType = eventTypeResolver.GetEventType(EventType);
                return (IEvent) EventData.ToObject(eventType);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to convert to an Event. Make sure the event " +
                                    "class is in the correct namespace.");
            }
        }
    }
}