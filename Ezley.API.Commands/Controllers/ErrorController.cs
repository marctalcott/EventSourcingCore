using System;
using Ezley.API.Commands.Infrastructure;
using Ezley.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Ezley.API.Commands.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseController
    {
        [Route("error")]
        public ErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error; // Your exception
            var code = 500; // Internal Server Error by default
            var type = "Server Error";

            if (exception is NotFoundException)
            {
                code = 404;
                type = "Not Found";
            }
            else if (exception is ApplicationException) {
                code = 400;
                type = "Bad Request";
            }
            Response.StatusCode = code;
            return new ErrorResponse(code,type, exception.Message); // Your error model
        }
    }
}