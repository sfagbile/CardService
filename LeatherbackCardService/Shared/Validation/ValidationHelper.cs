using System;

namespace Shared.Validation
{
    public class ValidationHelper
    {
        public static bool IsValidGuid(Guid guidString)
        {
            if (guidString == default)
                return false;
            return true;
        }
    }
}