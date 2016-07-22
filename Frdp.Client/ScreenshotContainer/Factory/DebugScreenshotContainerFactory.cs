using System;
using FrameworkExtensions;
using Frdp.Common;

namespace Frdp.Client.ScreenshotContainer.Factory
{
    internal class DebugScreenshotContainerFactory : IScreenshotContainerFactory
    {
        private readonly IScreenshotContainerFactory _factory;
        private readonly ILogger _logger;

        public DebugScreenshotContainerFactory(
            IScreenshotContainerFactory factory,
            ILogger logger
            )
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            _factory = factory;
            _logger = logger;
        }

        public IScreenshotContainer TakeScreenShot()
        {
            var timer = new PerformanceTimer();

            var result = _factory.TakeScreenShot();

            var msecTaken = timer.MilliSeconds;
            _logger.LogFormattedMessage(
                "Msec taken for screenshot: {0}",
                msecTaken
                );

            return
                result;
        }
    }
}