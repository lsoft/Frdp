using System;
using System.Threading;
using Frdp.Client.ScreenshotContainer.Factory.Duplicate;
using Frdp.Common;
using Frdp.Common.Settings;

namespace Frdp.Client.ScreenshotContainer.Factory
{
    internal class DesktopDuplicationScreenshotContainerFactory : IScreenshotContainerFactory, IDisposable
    {
        private readonly IClientSettingsContainer _clientSettings;
        private readonly ILogger _logger;

        private DesktopDuplicator _desktopDuplicator;

        private bool _disposed = false;

        public DesktopDuplicationScreenshotContainerFactory(
            IClientSettingsContainer clientSettings,
            ILogger logger
            )
        {
            if (clientSettings == null)
            {
                throw new ArgumentNullException("clientSettings");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _clientSettings = clientSettings;
            _logger = logger;

            Recreate();
        }

        public IScreenshotContainer TakeScreenShot()
        {
            try
            {
                return
                    Take();
            }
            catch (Exception)
            {
                this.Recreate();

                //делаем еще одну попытку
                return
                    Take();
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                _desktopDuplicator.Dispose();
            }
        }

        private IScreenshotContainer Take()
        {
            DesktopFrame bitmap = null;

            for (var c = 0; c < 10 && bitmap == null; c++)
            {
                bitmap = _desktopDuplicator.GetLatestFrame();
            }

            if (bitmap == null)
            {
                throw new InvalidOperationException("bitmap is null");
            }

            var result = new DefaultScreenshotContainer(
                _clientSettings,
                _logger,
                bitmap.DesktopImage
                );

            return
                result;
        }

        private void Recreate()
        {
            try
            {
                var newdd = new DesktopDuplicator(0);

                var olddd = Interlocked.Exchange(ref _desktopDuplicator, newdd);

                if (olddd != null)
                {
                    olddd.Dispose();
                }
            }
            catch (Exception excp)
            {
                _logger.LogException(excp);
            }
        }

    }
}