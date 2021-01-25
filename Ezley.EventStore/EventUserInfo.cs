namespace Ezley.EventStore
{
    public class EventUserInfo
    {
        public string AuthServiceUserId { get; }

        public EventUserInfo(string authServiceUserId)
        {
            AuthServiceUserId = authServiceUserId;
        }
    }
}