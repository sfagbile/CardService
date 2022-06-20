using System;

namespace Domain.Exceptions
{
    public class InvalidCardException: Exception
    {
        public InvalidCardException(string message): base(message)
        {
            
        }
    }
}