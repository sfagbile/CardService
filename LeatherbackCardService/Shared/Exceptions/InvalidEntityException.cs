using System;

namespace Shared.Exceptions
{
    public class InvalidEntityException: Exception
    {
        public InvalidEntityException(string message): base(message)
        {
            
        }
    }
}