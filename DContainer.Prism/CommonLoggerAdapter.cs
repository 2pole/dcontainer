using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
using Prism.Logging;

namespace DContainer.Prism
{
    /// <summary>
    /// A adapter for common log.
    /// </summary>
    public class CommonLoggerAdapter : ILoggerFacade
    {
        public ILog Logger { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CommonLoggerAdapter(ILogManager logManager)
        {
            this.Logger = logManager.GetLogger<CommonLoggerAdapter>();
        }

        #region Implementation of ILoggerFacade

        /// <summary>
        /// Write a new log entry with the specified category and priority.
        /// </summary>
        /// <param name="message">Message body to log.</param><param name="category">Category of the entry.</param><param name="priority">The priority of the entry.</param>
        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    Logger.Debug(message);
                    break;
                case Category.Exception:
                    Logger.Error(message);
                    break;
                case Category.Info:
                    Logger.Info(message);
                    break;
                case Category.Warn:
                    Logger.Warn(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("category");
            }
        }

        #endregion
    }
}
