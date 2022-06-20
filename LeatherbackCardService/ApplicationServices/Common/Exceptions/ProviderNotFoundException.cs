using System;

namespace ApplicationServices.Common.Exceptions
{
    public class ProviderNotFoundException: Exception
    {
        public ProviderNotFoundException(string message): base(message)
        {
            
        }
    }
}