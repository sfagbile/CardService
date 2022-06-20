using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Shared.RandomNumber
{
    public static class GenerateRandomNumber
    {
        private static RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            return result;
        }

        public static bool IsValidGuid(Guid guidString)
        {
            if (guidString == default)
                return false;
            return true;
        }
        
        public static bool IsValidGuid(Guid? guidString)
        {
            if (guidString.HasValue == false || guidString.Value == default)
            {
                    return false;
            }

            return true;
        }


        /// <summary>
        /// Generate random numbers thar aren't predictable
        /// </summary>
        /// <param name="lengthOfBytes">Length of the Byte Array</param>
        /// <returns>Random number generated</returns>
        public static string GenerateRandomNumbers(int lengthOfBytes = 4)
        {
            var byteArray2 = new byte[lengthOfBytes];
            provider.GetBytes(byteArray2);

            //convert 8 bytes to a double
            var randomDouble = BitConverter.ToUInt32(byteArray2, 0);
            return randomDouble.ToString();
        }
    }
}