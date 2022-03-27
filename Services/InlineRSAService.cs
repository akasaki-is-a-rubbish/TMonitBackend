using System;
using System.Security.Cryptography;
using System.Text;

namespace TMonitBackend.Services
{
    public class InlineRSA
    {
        private static UnicodeEncoding _encoder = new UnicodeEncoding();

        public InlineRSA() { }
        protected static RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        protected static string _privateKey = RSA.ToXmlString(true);
        protected static string _publicKey = RSA.ToXmlString(false);
        public static RSAParameters ExportPublicKey() => RSA.ExportParameters(false);

        public static string Decrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            var dataArray = data.Split(new char[] { ',' });
            byte[] dataByte = new byte[dataArray.Length];
            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

            rsa.FromXmlString(_privateKey);
            var decryptedByte = rsa.Decrypt(dataByte, false);
            return _encoder.GetString(decryptedByte);
        }

        public static string Encrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_publicKey);
            var dataToEncrypt = _encoder.GetBytes(data);
            var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
            var length = encryptedByteArray.Count();
            var item = 0;
            var sb = new StringBuilder();
            foreach (var x in encryptedByteArray)
            {
                item++;
                sb.Append(x);

                if (item < length)
                    sb.Append(",");
            }

            return sb.ToString();
        }
    }
}