using System;

namespace Ezley.API.Commands.ViewModels
{
    public class Auth0RegistrationModel
    {
        public Auth0RegistrationModel(
            string clientId, string email, string password, string connection,
            Guid ezleyUserId, Guid tenantId, string application)
        {
            this.client_id = clientId;
            this.email = email;
            this.password = password;
            this.connection = connection;
            this.app_metadata = new Auth0AppMetaData(ezleyUserId, tenantId, application);
            
        }

        public string client_id { get; private set; }
        public string email { get; private set; }
        public string password { get; private set; }
        public string connection { get; private set; }

        public Auth0AppMetaData app_metadata { get; private set; }
    }

    // This is a property of an Auth0 Signup so that we can pass in the application userId of this person.
}