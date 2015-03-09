#region using
using DAL.Entities;
using System;
using System.Net;
using System.Net.Mail; 
#endregion

namespace BLL.Tools
{
    public class EmailProcessor
    {
        #region Fields&Props
        private EmailSettings settings;
        private User _fromUser;
        public User _toUser; 
        #endregion

        #region .ctors
        public EmailProcessor(User fromUser, User toUser, string email)
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("Email is null or empty", (Exception)null);
            if ((fromUser == null) || (toUser == null))
                throw new ArgumentNullException("User is null", (Exception)null);

            _fromUser = fromUser;
            _toUser = toUser;
            settings = new EmailSettings(email);
        } 
        #endregion

        #region Send
        public bool Send(string message)
        {
            throw new NotImplementedException();

            if (String.IsNullOrEmpty(message) || String.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException("Message is null or empty", (Exception)null);

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = settings.SSL;
                smtpClient.Host = settings.Server;
                smtpClient.Port = settings.Port;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(settings.Username, settings.Password);

                string str = String.Empty;
                MailMessage mailMessage = new MailMessage(settings.From, settings.To, "Say Social Network", str + message);
                smtpClient.Send(mailMessage);
                return true;
            }
        }
        #endregion
    }

    #region EmailSettings
    internal class EmailSettings
    {
        internal EmailSettings(string to)
        {
            To = to;
            Server = String.Empty;
            From = String.Empty;
            SSL = true;
            Username = String.Empty;
            Password = String.Empty;
            Port = 2525;
        }

        public string Server { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public bool SSL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    } 
    #endregion

}