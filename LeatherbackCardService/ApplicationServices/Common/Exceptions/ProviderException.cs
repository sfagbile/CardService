using System;

namespace ApplicationServices.Common.Exceptions
{
    public class ProviderException: Exception
    {
        public ProviderException(string message): base(message)
        {
            
        }
    }
}