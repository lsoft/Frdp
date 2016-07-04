using System;
using System.Drawing;
using Frdp.Client.Helpers;
using Frdp.Client.ScreenInfo.Factory;
using Frdp.Common;
using Frdp.Common.Settings;

namespace Frdp.Client.ScreenshotContainer.Factory
{
    public class DefaultScreenshotContainerFactory : IScreenshotContainerFactory
    {
        private readonly IScreenInfoFactory _screenInfoFactory;
        private readonly IClientSettingsContainer _clientSettings;
        private readonly ILogger _logger;

        private readonly object _lockObject = new object();

        public DefaultScreenshotContainerFactory(
            IScreenInfoFactory screenInfoFactory,
            IClientSettingsContainer clientSettings,
            ILogger logger
            )
        {
            if (screenInfoFactory == null)
            {
                throw new ArgumentNullException("screenInfoFactory");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _screenInfoFactory = screenInfoFactory;
            _clientSettings = clientSettings;
            _logger = logger;
        }

        public IScreenshotContainer TakeScreenShot(
            )
        {
            lock (_lockObject)
            {
                using (var si = _screenInfoFactory.CreateScreenInfo())
                {
                    var hDest = InvokeHelper.CreateCompatibleDC(si.WindowDC);
                    var hBmp = InvokeHelper.CreateCompatibleBitmap(si.WindowDC, si.Width, si.Height);
                    var hOldBmp = InvokeHelper.SelectObject(hDest, hBmp);

                    try
                    {
                        InvokeHelper.BitBlt(hDest, 0, 0, si.Width, si.Height, si.WindowDC, si.Left, si.Top, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt);

                        InvokeHelper.SelectObject(hDest, hOldBmp);

                        var bitmap = Image.FromHbitmap(hBmp);

                        var result = new DefaultScreenshotContainer(
                            _clientSettings,
                            _logger,
                            hBmp,
                            bitmap
                            );

                        return
                            result;
                    }
                    finally
                    {
                        InvokeHelper.DeleteObject(hBmp);
                        InvokeHelper.DeleteDC(hDest);
                    }
                }
            }
        }

    }
}
