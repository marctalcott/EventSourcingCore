using System;
using Ezley.EventSourcing;
using Ezley.ValueObjects;

namespace Ezley.Events
{
    public class UserPersonNameChanged:EventBase
    {
        public Guid Id { get; private set; }
        public PersonName PersonName { get; private set; }

        public UserPersonNameChanged(Guid id, PersonName personName)
        {
            Id = id;
            PersonName = personName;
        }
    }
}