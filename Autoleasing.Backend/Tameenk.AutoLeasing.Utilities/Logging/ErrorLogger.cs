using System;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace Tameenk.AutoLeasing.Utilities
{
    public class ErrorLogger
    {
      
        public static void Initialize(ILoggerRepository logger)
        {
            XmlConfigurator.Configure(logger);
        }
        /// <summary>
        /// 
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Log Error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="rethrowException"></param>
        public static void LogError(string message, Exception exception, bool rethrowException)
        {
            if (message != null && message.Length > 4000)
            {
                message = message.Substring(0, 4000);//4000 maximum size of Message field in DB
            }
            log.Error(message, exception);

            if (rethrowException)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Log Debug
        /// </summary>
        /// <param name="message"></param>
        public static void LogDebug(string message)
        {
            log.Debug(message);
        }
    }
}
