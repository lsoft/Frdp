using Frdp.Client.Helpers;
using Frdp.Common;

namespace Frdp.Client.ScreenInfo.Factory
{
    public class PrimaryMonitorScreenInfoFactory : IScreenInfoFactory
    {
        public IScreenInfo CreateScreenInfo()
        {
            var hDesk = InvokeHelper.GetDesktopWindow();
            var hSrce = InvokeHelper.GetWindowDC(hDesk);

            var width = InvokeHelper.GetDeviceCaps(hSrce, (int)InvokeHelper.DeviceCap.HORZRES);
            var height = InvokeHelper.GetDeviceCaps(hSrce, (int)InvokeHelper.DeviceCap.VERTRES);

            var szBounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;

            var left = szBounds.Left;
            var top = szBounds.Top;

            return 
                new HdcScreenInfo(
                    hDesk,
                    hSrce,
                    left,
                    top,
                    width,
                    height
                    );
        }
    }
}