using DAL.Entities;
using System;
using System.Net;
using System.Net.Mail;

namespace BLL.Tools
{
    public class EmailProcessor
    {
        private EmailSettings settings;
        private User _fromUser;
        public User _toUser;

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
            if (String.IsNullOrEmpty(message) || String.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException("Message is null or empty", (Exception)null);

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = settings.SSL;
                smtpClient.Host = settings.Server;
                smtpClient.Port = settings.Port;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(settings.Username, settings.Password);

                string str = "Hello, dear " + _toUser.FirstName + "! "
                    + _fromUser.FirstName + " " + _fromUser.Surname +
                    " has writed a message to you" + Environment.NewLine + Environment.NewLine;
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
            Server = "smtp.mail.ru";
            From = "SaySocialNetwork@mail.ru";
            SSL = true;
            Username = "SaySocialNetwork@mail.ru";
            Password = "say_rpr2782451UIjka";
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