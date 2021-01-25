// using System;
//
// namespace Ezley.API.Commands.ViewModels
// {
//     public class Auth0UserMetaData
//     {
//         public Auth0UserMetaData(Guid ezleyUserId, Guid tenantId, string application)
//         {
//             // User Id is required to create a new registration so we can map the users token to a user without
//             // doing extra lookups
//             if (ezleyUserId == null || ezleyUserId == Guid.Empty)
//             {
//                 throw new ApplicationException("Invalid user id.");
//             }
//
//             Id = ezleyUserId;
//             TenantId = tenantId;
//             Application = application;
//         }
//
//        //  public Guid Id { get; private set; }
//        // public Guid TenantId { get; private set; }
//        //  public string Application { get; private set; }
//     }
// }