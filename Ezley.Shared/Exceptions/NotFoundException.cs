using System;

namespace Ezley.Shared.Exceptions
{
    public class NotFoundException:Exception
    {
        public NotFoundException()
        {
            
        }
        public NotFoundException(string message): base(message)
        {
        }
    }
}