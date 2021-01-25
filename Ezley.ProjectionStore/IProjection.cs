using Ezley.EventSourcing;

namespace Ezley.ProjectionStore
{
    public interface IProjection
    {
        bool IsSubscribedTo(IEvent @event);

        string[] GetViewNames(string streamId, IEvent @event);

        void Apply(IEvent @event, IView view);
    }
}