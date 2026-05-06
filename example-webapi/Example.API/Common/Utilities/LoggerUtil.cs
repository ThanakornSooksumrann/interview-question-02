using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.API.Common.Utilities
{
    public class LoggerUtil
    {
        public static void GetLoggerThreadId()
        {
            if (ThreadContext.Properties["threadid"] == null)
            {
                ThreadContext.Properties["threadid"] = Thread.CurrentThread.ManagedThreadId;
            }
        }
    }
}
