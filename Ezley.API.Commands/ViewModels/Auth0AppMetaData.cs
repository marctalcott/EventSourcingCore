using System;

namespace Ezley.API.Commands.ViewModels
{
    public class Auth0AppMetaData
    {
        public Auth0AppMetaData(Guid ezleyUserId, Guid tenantId, string application)
        {
            // User Id is required to create a new registration so we can map the users token to a user without
            // doing extra lookups
            if (ezleyUserId == Guid.Empty)
            {
                throw new ApplicationException("Invalid user id.");
            }

            EzleyUser = ezleyUserId;
            EzleyTenant = tenantId;
            EzleyApp = application;
        }

        public Guid EzleyUser { get; private set; }
        public Guid EzleyTenant { get; private set; }
        public string EzleyApp { get; private set; }
    }
}