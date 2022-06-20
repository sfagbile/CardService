using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Shared.Encryption
{
    public class RSAEncryption
    {
        public string Encrypt(string plainTextData, string publicKey)
        {
            try
            {
                RSACryptoServiceProvider rsaCryptoServiceProvider = ImportPublicKey(publicKey);
                //for encryption, always handle bytes...
                var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(plainTextData);

                //apply pkcs#1.5 padding and encrypt our data 
                var bytesCypherText = rsaCryptoServiceProvider.Encrypt(bytesPlainTextData, false);

                //we might want a string representation of our cypher text... base64 will do
                var cypherText = Convert.ToBase64String(bytesCypherText);

                System.Diagnostics.Debug.WriteLine("cypherText : " + cypherText);

                return cypherText;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("err : " + e.StackTrace);
                return null;
            }
        }


        public string Decrypt(string cypherText, string privateKey)
        {
            RSACryptoServiceProvider rsaCryptoServiceProvider = ImportPrivateKey(privateKey);

            //first, get our bytes back from the base64 string ...
            var bytesCypherText = Convert.FromBase64String(cypherText);

            //we want to decrypt, therefore we need a csp and load our private key
            //decrypt and strip pkcs#1.5 padding
            var bytesPlainTextData = rsaCryptoServiceProvider.Decrypt(bytesCypherText, false);

            //get our original plainText back...
            var plainTextData = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);

            System.Diagnostics.Debug.WriteLine("DecryptData : " + plainTextData);
            return plainTextData;
        }
        
        public static RSACryptoServiceProvider ImportPrivateKey(string pem)
        {
            PemReader pr = new PemReader(new StringReader(pem));
            AsymmetricCipherKeyPair KeyPair = (AsymmetricCipherKeyPair) pr.ReadObject();
            RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters) KeyPair.Private);

            RSACryptoServiceProvider csp = new RSACryptoServiceProvider(); // cspParams);
            csp.ImportParameters(rsaParams);
            return csp;
        }

        public static RSACryptoServiceProvider ImportPublicKey(string pem)
        {
            PemReader pr = new PemReader(new StringReader(pem));
            AsymmetricKeyParameter publicKey = (AsymmetricKeyParameter) pr.ReadObject();
            RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters) publicKey);

            RSACryptoServiceProvider csp = new RSACryptoServiceProvider(); // cspParams);
            csp.ImportParameters(rsaParams);
            return csp;
        }
    }
}