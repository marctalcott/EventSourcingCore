using System;

namespace Ezley.API.Commands.Infrastructure
{
    public class ErrorResponse
    {
        public int Status { get; private set; }
        public string Type { get; private set; }
        public string Message { get; private set; }

        public ErrorResponse(int status, string type, string message)
        {
            Status = status;
            Type = type;
            Message = message;
        }
    }
}