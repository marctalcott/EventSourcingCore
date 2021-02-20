using System.Linq;
using Ezley.EventStore;
using Microsoft.AspNetCore.Mvc;

namespace Ezley.API.Commands.Controllers
{  
     
    public class BaseController : ControllerBase
    {
        public BaseController()
        {
        }
        
        protected EventUserInfo GetUserInfo()
        {
            var name = GetNameFromToken();
            return new EventUserInfo(name);
        }

        private string GetNameFromToken()
        {
            return "anonymous";
            // Use below when using Auth0 tokens
            // string claimsId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            //
            // var user = HttpContext.User;
            // string nameIdentifier = user.Claims.FirstOrDefault(x =>
            //     x.Type == claimsId)?.Value;
            //
            // return nameIdentifier;
        }
    }
 
}