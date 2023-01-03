using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public enum ErrorType
    {
        Error,
        Warning,
        Information
    }
    public class ErrorHandler
    {
        private string _logName;
        private string _logSource;

        public ErrorHandler(string logName, string logSource)
        {
            _logName = logName;
            _logSource = logSource;
        }

        public void LogError(String msg, ErrorType type)
        {
            try
            {
                if (!EventLog.SourceExists(_logSource))
                    EventLog.CreateEventSource(_logSource, _logName);

                if (type == ErrorType.Information)
                    EventLog.WriteEntry(_logSource, msg, EventLogEntryType.Information, 1);
                else if (type == ErrorType.Warning)
                    EventLog.WriteEntry(_logSource, msg, EventLogEntryType.Warning, 1);
                else
                    EventLog.WriteEntry(_logSource, msg, EventLogEntryType.Error, 1);
            }
            catch { }
        }

    }
}
