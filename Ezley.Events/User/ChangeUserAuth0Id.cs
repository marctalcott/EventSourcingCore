using System;
using Ezley.EventSourcing;

namespace Ezley.Events
{
    public class UserAuth0IdChanged:EventBase
    {
        public Guid Id { get; private set; }
        public string Auth0Id { get; private set; }

        public UserAuth0IdChanged(Guid id, string auth0Id)
        {
            Id = id;
            Auth0Id = auth0Id;
        }
    }
}