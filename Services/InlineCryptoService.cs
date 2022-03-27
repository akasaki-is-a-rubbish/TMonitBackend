using System;
using System.Security.Cryptography;
using System.Text;

namespace TMonitBackend.Services
{
    public class InlineCrypto
    {
        private static UnicodeEncoding _encoder = new UnicodeEncoding();

        public InlineCrypto() { }
        protected static RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        protected static string _privateKey = RSA.ToXmlString(true);
        protected static string _publicKey = RSA.ToXmlString(false);
        public static RSAParameters ExportPublicKey() => RSA.ExportParameters(false);

        public static string RSADecrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            // var dataArray = data.Split(new char[] { ',' });
            // byte[] dataByte = data.Split(',').Select(s => Convert.ToByte(s)).ToArray();
            byte[] dataByte = Convert.FromBase64String(data);
            rsa.FromXmlString(_privateKey);
            var decryptedByte = rsa.Decrypt(dataByte, false);
            return _encoder.GetString(decryptedByte);
        }

        public static string RSAEncrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_publicKey);
            var dataToEncrypt = _encoder.GetBytes(data);
            var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
            return Convert.ToBase64String(encryptedByteArray);
        }

        public static string Base64Encode(string plainText)
            => System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));

        public static string Base64Decode(string base64EncodedData)
            => System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(base64EncodedData));
    }
}