using System;
using Ezley.ValueObjects;
using Newtonsoft.Json;

namespace Ezley.Domain.CRM
{
    public class TenantSnapshot
    {
        [JsonProperty]
        public Guid Id
        {
            get => _id;
            private set => _id = value;
        }
        [JsonProperty]
        public string LegalName
        {
            get => _legalName;
            private set => _legalName = value;
        }
        [JsonProperty]
        public string DisplayName
        {
            get => _displayName;
            private set => _displayName = value;
        }
        [JsonProperty]
        public Address Address
        {
            get => _address;
            private set => _address = value;
        }
        [JsonProperty]
        public Phone Phone
        {
            get => _phone;
            private set => _phone = value;
        }
        [JsonProperty]
        public Email Email
        {
            get => _email;
            private set => _email = value;
        }
        [JsonProperty]
        public bool Active
        {
            get => _active;
            private set => _active = value;
        }
        [JsonProperty]
        public int Version
        {
            get => _version;
            private set => _version = value;
        }

        private Guid _id;
        private string _legalName;
        private string _displayName;
        private Address _address;
        private Phone _phone;
        private Email _email;
        private bool _active;
        private int _version;

        public TenantSnapshot(Guid id,string legalName, string displayName,
            Address address, Phone phone, Email email, bool active, int version)
        {
            _id = id;
            _legalName = legalName;
            _displayName = displayName;
            _address = address;
            _phone = phone;
            _email = email;
            _active = active;
            _version = version;
        }
    }
}