using System;
using System.IO;
using System.Linq;
using CLogger;
using CLogger.DirectLogger;
using CLogger.Gate;

namespace Frdp.Common
{
    public class CombinedLogger : ILogger, IDisposable
    {
        private const string JournalLogPath = ".\\Log";

#if !WindowsCE
        //private const uint JournalLogMaxFileCount = 8;
        private const uint JournalLogMaxFileSize = 16777216;
#else
    //private const uint JournalLogMaxFileCount = 16;
        private const uint JournalLogMaxFileSize = 512*1024;
#endif
        private const short StackTraceFilterLevel = 5;

        private readonly IGateLogging _gateLogger;

        public CombinedLogger(
            uint journalLogMaxFileCount,
            bool isServiceMode,
            bool isNeedToZipLogFiles
#if WindowsCE
            ,Func<DateTime> nowProvider
#endif
            )
        {
#if WindowsCE
            if (nowProvider == null)
            {
                throw new ArgumentNullException("nowProvider");
            }
#endif
            var dln = new DirectoryInfo(JournalLogPath).Name;

            if (!Directory.Exists(Path.GetFullPath(dln)))
            {
                Directory.CreateDirectory(Path.GetFullPath(dln));
            }

            var sLogFileFormat = string.Format(
                "{0}\\{1}.{{0}}.{{1}}.log",
                Path.GetFullPath(JournalLogPath),
                Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName));

            var gateLogger = new GateLoggingObject(
#if WindowsCE
                nowProvider
#endif
                );

            if (isNeedToZipLogFiles)
            {
                var fileLogger = new ZipFileDirectMessageLogger(
                    sLogFileFormat,
                    journalLogMaxFileCount,
                    JournalLogMaxFileSize,
                    StackTraceFilterLevel
                    );

                gateLogger.AddMessageLogger(fileLogger);
            }
            else
            {
                var fileLogger = new FileDirectMessageLogger(
                    sLogFileFormat,
                    journalLogMaxFileCount,
                    JournalLogMaxFileSize,
                    StackTraceFilterLevel
                    );

                gateLogger.AddMessageLogger(fileLogger);
            }

#if !CF4Win32
            if (!isServiceMode)
            {
                var textLogger = new TextWriterDirectMessageLogger(
                    StackTraceFilterLevel
                    );

                gateLogger.AddMessageLogger(textLogger);

            }
#endif

            _gateLogger = gateLogger;
        }

        public void LogFormattedMessage(string message, params object[] parameters)
        {
            if (parameters == null)
            {
                LogMessage(message);
            }

            _gateLogger.LogMessage(
                LogMessageCategories.Info,
                this.ExtractMethodNameFromStack(),
                string.Format(
                    message,
                    parameters));
        }

        public void LogMessage(string message)
        {
            LogMessage(
                message,
                null
                );
        }

#if !WindowsCE
        public void LogMessage(string message, string source = null)
#else
        public void LogMessage(string message, string source)
#endif
        {
            if (source == null)
            {
                source = this.ExtractMethodNameFromStack();
            }

            _gateLogger.LogMessage(
                LogMessageCategories.Info,
                source,
                message);
        }

        public void LogException(Exception excp)
        {
            LogException(excp, string.Empty);
        }

#if !WindowsCE
        public void LogException(Exception excp, string message = "")
        {
            _gateLogger.LogHandledException(
                LogMessageCategories.Error,
                this.ExtractMethodNameFromStack(),
                message,
                excp);
        }
#else
        public void LogException(Exception excp, string message)
        {
            _gateLogger.LogHandledException(
                LogMessageCategories.Error,
                this.ExtractMethodNameFromStack(),
                message,
                excp);
        }

        public void LogException(Exception excp)
        {
            this.LogException(excp, string.Empty);
        }
#endif


        private string ExtractMethodNameFromStack()
        {
            var result = "-source_does_not_found-";

#if !WindowsCE
            var trace = new System.Diagnostics.StackTrace();

            foreach (var f in trace.GetFrames().Skip(1))
            {
                var m = f.GetMethod();
                var t = m.DeclaringType;

                if (t.GetInterface(typeof(ILogger).Name) == null)
                {
                    result = string.Format(
                        "{0}.{1}",
                        t.Name,
                        m.Name);

                    break;
                }
            }

            if (result.Length < 32)
            {
                result += new string(' ', 32 - result.Length);
            }
#else
            result = FrameworkExtensions.Environment.CallerClassAndMethodName;
#endif

            return result;
        }

        public void Dispose()
        {
            _gateLogger.CloseLogger();
        }
    }
}