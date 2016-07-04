using System;
using OpenCvSharp;

namespace Frdp.Client.ScreenshotContainer
{
    public interface IScreenshotContainer : IDisposable
    {
        Mat ConvertToMinimizedMat();
        
        void Save(
            string filepath
            );
    }
}