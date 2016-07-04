using System;
using System.Drawing;
using System.Drawing.Imaging;
using Frdp.Client.Helpers;
using Frdp.Common;
using Frdp.Common.Settings;
using OpenCvSharp;

namespace Frdp.Client.ScreenshotContainer
{
    public class DefaultScreenshotContainer : IScreenshotContainer
    {
        private readonly Bitmap _bitmap;
        private readonly IClientSettingsContainer _clientSettings;

        private readonly IntPtr _hBmp;
        private readonly ILogger _logger;

        private volatile bool _disposed;

        public DefaultScreenshotContainer(
            IClientSettingsContainer clientSettings,
            ILogger logger,
            IntPtr hBmp,
            Bitmap bitmap
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
            if (bitmap == null)
            {
                throw new ArgumentNullException("bitmap");
            }

            _clientSettings = clientSettings;
            _logger = logger;
            _hBmp = hBmp;
            _bitmap = bitmap;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                try
                {
                    InvokeHelper.DeleteObject(_hBmp);
                }
                catch
                {
                    //nothing to do, suppress this exception
                }

                _disposed = true;
            }
        }

        public Mat ConvertToMinimizedMat(
            )
        {
            //_logger.LogFormattedMessage(
            //    "Pixel format: {0}",
            //    _bitmap.PixelFormat
            //    );

            using (Mat prem = OpenCVHelper.ImprovedToMat(_bitmap)) // _bitmap.ToMat())
            {
                //convert to grayscale
                using (var m0 = new Mat(prem.Size(), MatType.CV_8UC1))
                {
                    if (_bitmap.PixelFormat == PixelFormat.Format16bppRgb565)
                    {
                        Cv2.CvtColor(
                            prem,
                            m0,
                            ColorConversionCodes.BGR5652GRAY
                            );
                    }
                    else
                    {
                        Cv2.CvtColor(
                            prem,
                            m0,
                            ColorConversionCodes.BGR2GRAY //BGR5652GRAY
                            );
                    }

                    //resize
                    var m1 = new Mat(m0.Height/_clientSettings.ScaleFactorY, m0.Width/_clientSettings.ScaleFactorX, m0.Type());

                    Cv2.Resize(
                        m0,
                        m1,
                        m1.Size(),
                        0,
                        0,
                        InterpolationFlags.Linear
                        );

                    return
                        m1;
                }
            }
        }

        public void Save(string filepath)
        {
            if (filepath == null)
            {
                throw new ArgumentNullException("filepath");
            }

            _bitmap.Save(
                filepath
                );
        }
    }
}