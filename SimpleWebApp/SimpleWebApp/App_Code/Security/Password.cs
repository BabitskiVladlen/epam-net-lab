using System;
using System.Security.Cryptography;
using System.Text;

namespace SimpleWebApp.Security
{
    public static class Password
    {
        public static string Create(string plainText)
        {
            if (String.IsNullOrEmpty(plainText) || String.IsNullOrWhiteSpace(plainText))
                throw new ArgumentNullException("Password is null or empty", (Exception)null);
            return Encrypt(plainText);
        }


        public static bool Verify(string plainText,  string password)
        {

            if (String.IsNullOrEmpty(password) || String.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password is null or empty", (Exception)null);
            if (String.IsNullOrEmpty(plainText) || String.IsNullOrWhiteSpace(plainText))
                throw new ArgumentNullException("Password is null or empty", (Exception)null);

            return (Create(plainText) == password);
        }

        #region Encrypt
        private static string Encrypt(string plainText)
        {
            byte[] data;
            using (MD5 md5 = MD5.Create())
            {
                data = md5.ComputeHash(Encoding.UTF8.GetBytes(plainText));
            }
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < data.Length; ++i)
                str.Append(data[i].ToString("x2"));
            return str.ToString();
        }  
        #endregion
    }
}