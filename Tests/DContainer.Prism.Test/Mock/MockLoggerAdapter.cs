using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prism.Logging;

namespace DContainer.Prism.Test.Mock
{
    internal class MockLoggerAdapter : ILoggerFacade
    {
        public IList<string> Messages = new List<string>();

        public void Log(string message, Category category, Priority priority)
        {
            Messages.Add(message);
        }
    }
}
