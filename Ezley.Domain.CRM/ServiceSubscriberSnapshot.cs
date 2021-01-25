using System;
using Ezley.ValueObjects;
using Newtonsoft.Json;

namespace Ezley.Domain.CRM
{
    public class ServiceSubscriberSnapshot
    {
        [JsonProperty] public Guid ServiceSubscriberId { get; private set; }

        [JsonProperty]
        public Guid Id { get; private set; }
        [JsonProperty]
        public string LegalName { get; private set; }
        [JsonProperty]
        public string DisplayName { get; private set; }
        [JsonProperty]
        public Address Address { get; private set; }
        [JsonProperty]
        public Phone Phone { get; private set; }
        [JsonProperty]
        public Email Email { get; private set; }
        [JsonProperty]
        public bool Active { get; private set; }
        [JsonProperty]
        public int Version { get; private set; }

       
        public ServiceSubscriberSnapshot(Guid id, Guid serviceSubscriberId, string legalName, string displayName,
            Address address, Phone phone, Email email, bool active, int version)
        {
            Id = id;
            ServiceSubscriberId = serviceSubscriberId;
            LegalName = legalName;
            DisplayName = displayName;
            Address = address;
            Phone = phone;
            Email = email;
            Active = active;
            Version = version;
        }
    }
}