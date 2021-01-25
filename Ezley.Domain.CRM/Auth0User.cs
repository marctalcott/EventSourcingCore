using System;
using System.Collections.Generic;
using ES.Domain;
using Ezley.Events;
using Ezley.EventSourcing;

namespace Ezley.Domain.CRM
{
    public class Auth0User: AggregateBase
    {
        public string Id { get; private set; }
        public Guid UserId { get; private set; }

        public Auth0User(IEnumerable<IEvent> events): base(events)
        {
        }
        // public Auth0User(string id, Guid userId)
        // {
        //     Apply(new Auth0UserRegistered(id, userId));
        // }
        // public void ChangeUserId(Guid userId)
        // {
        //     if(UserId == Guid.Empty)
        //         this.Apply(new Auth0UserUserIdChanged(this.Id, userId));
        // }
        //
        
        
        #region WhenEvent_ApplyEventsOnly
        protected override void Mutate(IEvent @event)
        {
            ((dynamic) this).When((dynamic) @event);
        }
        // protected void When(Auth0UserRegistered @event)
        // {
        //     Id = @event.Id;
        //     UserId = @event.UserId;
        //    
        // }
        // protected void When(Auth0UserUserIdChanged @event)
        // {
        //     UserId = @event.UserId;
        // }
        #endregion
    }
}