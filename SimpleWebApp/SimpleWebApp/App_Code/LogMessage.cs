using NLog;
using System;
using System.Text;

namespace SimpleWebApp
{
    public enum Level { Trace, Debug, Info, Warn, Error, Fatal };
    public static class LogMessage
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static void Add(Exception exc, Level level)
        {
            if (exc == null) return;
            StringBuilder message = new StringBuilder();
            message.Append("\r\n**************************************************************\r\n");

            #region InnerException
            if (exc.InnerException != null)
            {
                message.Append("Inner exception type: ");
                message.Append(exc.InnerException.GetType().ToString());
                message.Append("\r\nInner exception: ");
                message.Append(exc.InnerException.Message);
                message.Append("\r\nInner source: ");
                message.Append(exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    message.Append("\r\nInner Stack Trace:\r\n");
                    message.Append(exc.InnerException.StackTrace);
                    message.Append("\r\n");
                }
            }
            #endregion

            #region Exception
            message.Append("Exception type: ");
            message.Append(exc.GetType().ToString());
            message.Append("\r\nException: ");
            message.Append(exc.Message);
            message.Append("\r\nSource: ");
            message.Append(exc.Source);
            if (exc.StackTrace != null)
            {
                message.Append("\r\nStack Trace:\r\n");
                message.Append(exc.StackTrace);
                message.Append("\r\n");
            }
            #endregion

            Add(message.ToString(), level);
        }


        public static void Add(string message, Level level)
        {
            switch (level)
            {
                case Level.Debug: _logger.Debug(message);
                    break;
                case Level.Info: _logger.Info(message);
                    break;
                case Level.Warn: _logger.Warn(message);
                    break;
                case Level.Error: _logger.Error(message);
                    break;
                case Level.Fatal: _logger.Fatal(message);
                    break;
                default: _logger.Trace(message);
                    break;
            }
        }
    }
}