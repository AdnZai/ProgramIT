using System;
using System.Security.Cryptography;
using System.Text;

namespace ProgramIT.Controllers
{
    public class SzyfrowanieTekstu
    {
        TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
        MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

        private byte[] MD5Hash(string value)
        {
            return MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value));
        }

        public string Encrypt(string StringInput, string Keys)
        {
            DES.Key = MD5Hash(Keys);
            DES.Mode = CipherMode.ECB;
            byte[] buffer = UTF8Encoding.UTF8.GetBytes(StringInput);

            return Convert.ToBase64String(DES.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
        }


        public string Decrypt(string Enctyptedstring, string Key)
        {
            DES.Key = MD5Hash(Key);
            DES.Mode = CipherMode.ECB;

            byte[] Buffer = Convert.FromBase64String(Enctyptedstring);
            return UTF8Encoding.UTF8.GetString(DES.CreateDecryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

    }
}
