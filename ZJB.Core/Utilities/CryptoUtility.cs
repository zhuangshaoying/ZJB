using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZJB.Core.Utilities
{
    public class CryptoUtility
    {
        public static string TripleDESEncrypt(string beforeEncrypt)
        {
            try
            {
                byte[] key = Convert.FromBase64String(ConfigUtility.GetValue("TRIPLE_DES_KEY"));
                byte[] iv = Convert.FromBase64String(ConfigUtility.GetValue("TRIPLE_DES_IV"));

                return TripleDESEncrypt(beforeEncrypt, key, iv);
            }
            catch (Exception ex)
            {
                throw new CryptoException(
                    "Please configure base64 encoded TRIPLE_DES_KEY (24 bytes) and TRIPLE_DES_IV (8 bytes) in appSettings",
                    ex);
            }
        }

        public static string TripleDESDecrypt(string beforeDecrypt)
        {
            try
            {
                byte[] key = Convert.FromBase64String(ConfigUtility.GetValue("TRIPLE_DES_KEY"));
                byte[] iv = Convert.FromBase64String(ConfigUtility.GetValue("TRIPLE_DES_IV"));

                return TripleDESDecrypt(beforeDecrypt, key, iv);
            }
            catch (Exception ex)
            {
                throw new CryptoException(
                    "Please configure base64 encoded TRIPLE_DES_KEY (24 bytes) and TRIPLE_DES_IV (8 bytes) in appSettings",
                    ex);
            }
        }

        public static string TripleDESEncrypt(string str, byte[] key, byte[] iv)
        {
            return TripleDESEncrypt(str, Encoding.UTF8, key, iv);
        }

        public static string TripleDESEncrypt(string str, Encoding encoding, byte[] key, byte[] iv)
        {
            byte[] beforeEncrypt = encoding.GetBytes(str);
            byte[] afterEncrypt = TripleDESEncrypt(beforeEncrypt, key, iv);
            return Convert.ToBase64String(afterEncrypt);
        }

        public static byte[] TripleDESEncrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (MemoryStream encrypted = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(encrypted,
                 new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv),
                 CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                    return encrypted.ToArray();

                }
            }
        }

        public static string TripleDESDecrypt(string str, byte[] key, byte[] iv)
        {
            return TripleDESDecrypt(str, Encoding.UTF8, key, iv);
        }

        public static string TripleDESDecrypt(string str, Encoding encoding, byte[] key, byte[] iv)
        {
            byte[] beforeDecrypt = Convert.FromBase64String(str);
            byte[] afterDecrypt = TripleDESDecrypt(beforeDecrypt, key, iv);
            return encoding.GetString(afterDecrypt, 0, afterDecrypt.Length);
        }

        public static byte[] TripleDESDecrypt(byte[] data, byte[] key, byte[] iv)
        {
            byte[] buffer = new byte[1024];
            int count;

            using (MemoryStream encrypted = new MemoryStream(data))
            using (CryptoStream cryptoStream = new CryptoStream(encrypted,
              new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv),
              CryptoStreamMode.Read))
            using (MemoryStream decrypted = new MemoryStream())
            {
                while ((count = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    decrypted.Write(buffer, 0, count);
                }
                return decrypted.ToArray();
            }
        }
    }

    public class CryptoException : Exception
    {
        public CryptoException(string message, Exception ex)
            : base(message, ex)
        {
        }
        public CryptoException(string message)
            : base(message)
        {
        }
    }

}
