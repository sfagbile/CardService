using System;

namespace Domain.Exceptions
{
    public class DuplicateException: Exception
    {
        public DuplicateException(string message): base(message)
        {
            
        }
    }
}