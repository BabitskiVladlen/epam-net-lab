using BLL.Security.Infrastructure;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Security
{
    public class PasswordEngineMD5 : IPasswordEngine
    {
        #region Create
        public string Create(string plainText)
        {
            if (String.IsNullOrEmpty(plainText) || String.IsNullOrWhiteSpace(plainText))
                throw new ArgumentNullException("Password is null or empty", (Exception)null);
            return Encrypt(plainText);
        } 
        #endregion

        #region Verify
        public bool Verify(string plainText, string password)
        {

            if (String.IsNullOrEmpty(password) || String.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password is null or empty", (Exception)null);
            if (String.IsNullOrEmpty(plainText) || String.IsNullOrWhiteSpace(plainText))
                throw new ArgumentNullException("Password is null or empty", (Exception)null);

            return (Create(plainText) == password);
        } 
        #endregion

        #region Encrypt
        private string Encrypt(string plainText)
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