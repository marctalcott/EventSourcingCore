using Newtonsoft.Json.Linq;

namespace Ezley.ProjectionStore
{
    public interface IView
    {
        JObject Payload { get; set; }
    }
}