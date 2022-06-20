using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Shared.Exceptions;

namespace Shared.Utility
{
    public static class EncryptionUtil
    {
        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        
        public static string GenerateSha512Hash(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new ArgumentIsNullException($"{nameof(inputString)} should not be empty when generating SHA512 hash.");
            }
            
            using var sha512Hash = SHA512.Create();

            //From string to byte array
            var sourceBytes = Encoding.UTF8.GetBytes(inputString);
            var hashBytes = sha512Hash.ComputeHash(sourceBytes);
            var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

            return hash;
        }

        public static string GenerateSha256Hash(string inputString)
        {
            using var sha256Hash = SHA256.Create();

            //From string to byte array
            var sourceBytes = Encoding.UTF8.GetBytes(inputString);
            var hashBytes = sha256Hash.ComputeHash(sourceBytes);
            var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

            return hash;
        }

        public static string GenerateHmacSha512Hash(string inputString, string secretKey)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new ArgumentIsNullException($"{nameof(inputString)} should not be empty when generating HmacSha512 hash.");
            }

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentIsNullException($"{nameof(secretKey)} should not be empty when generating HmacSha512 hash.");
            }

            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            using var hmac = new HMACSHA512(secretKeyBytes);

            var inputBytes = Encoding.UTF8.GetBytes(inputString);
            var hashValue = hmac.ComputeHash(inputBytes);
            var hash = new StringBuilder();

            foreach (var theByte in hashValue)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }

        public static string ConvertToBase64String(string inputString)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(inputString));
        }

        public static string ConvertFromBase64String(string base64String)
        {
            var bytes = Convert.FromBase64String(base64String);

            return Encoding.UTF8.GetString(bytes);
        }
    }
}
