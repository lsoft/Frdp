using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frdp.Common
{
    public interface ILogger
    {
        void LogException(
            Exception excp
            );

        void LogException(
            Exception excp,
            string message
            );

        void LogMessage(
            string message
            );

        void LogFormattedMessage(
            string message,
            params object[] args
            );
    }
}
