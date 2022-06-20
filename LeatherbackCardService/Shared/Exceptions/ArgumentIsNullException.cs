using System;

namespace Shared.Exceptions
{
    public class ArgumentIsNullException: Exception
    {
        public ArgumentIsNullException(string message): base(message)
        {
        }
    }
}