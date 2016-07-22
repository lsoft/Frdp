using System;

namespace Frdp.Client.ScreenshotContainer.Factory.Duplicate
{
    public class DesktopDuplicationException : Exception
    {
        public DesktopDuplicationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DesktopDuplicationException(string message)
            : base(message)
        {
        }
    }
}