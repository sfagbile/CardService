namespace Shared.Helpers
{
    public static class PhoneNumberConfig
    {
        public static  (string prefix, string phoneNumber) GetPhoneNumberAndPrefix(string prefixPlusPhoneNumber)
        {
            var prefix = prefixPlusPhoneNumber.Substring(0, 3);
            var phoneNumber = prefixPlusPhoneNumber.Substring(3);
            return (prefix, phoneNumber);
        }
    }
}