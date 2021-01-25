using System;
using System.Collections.Generic;
using Ezley.EventSourcing;
using Newtonsoft.Json.Linq;

namespace Ezley.ProjectionStore
{
    public abstract class Projection<TView> : IProjection
        where TView : new()
    {
        private readonly Dictionary<Type, Action<IEvent, object>> _handlers;

        public Projection()
        {
            _handlers = new Dictionary<Type, Action<IEvent, object>>();
        }

        public bool IsSubscribedTo(IEvent @event) => _handlers.ContainsKey(@event.GetType());

        public virtual string[] GetViewNames(string streamId, IEvent @event) =>
            new string[] { $"{typeof(TView).Name}:{streamId}"};
        
        public void Apply(IEvent @event, IView view)
        {
            var payload = view.Payload.ToObject<TView>();

            var eventType = @event.GetType();
            if (_handlers.TryGetValue(eventType, out Action<IEvent, object> handler))
            {
                handler(@event, payload);

                view.Payload = JObject.FromObject(payload);
            }
        }

        protected void RegisterHandler<TEvent>(Action<TEvent, TView> handler)
            where TEvent : IEvent
        {
            _handlers[typeof(TEvent)] = new Action<IEvent, object>((e, v) => handler((TEvent)e, (TView)v));
        }
    }
}