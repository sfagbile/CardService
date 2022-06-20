using System;
using System.Text.RegularExpressions;

namespace Shared.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveSpecialCharacters(this string value)
        {
            var newValue= Regex.Replace(value, @"[^a-zA-Z- ]+", "");

            return newValue;
        }
        
        public static bool IsValidBase64String(this string base64)
        {
            try
            {
                _ = Convert.FromBase64String(base64);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
