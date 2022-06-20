using System;

namespace Domain.Exceptions
{
    public class InvalidMoneyMobileDetailException : Exception
    {
        public InvalidMoneyMobileDetailException(string message): base(message) 
        {
            
        }
    }
}