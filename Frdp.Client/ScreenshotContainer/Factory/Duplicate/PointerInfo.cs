using SharpDX;
using SharpDX.DXGI;

namespace Frdp.Client.ScreenshotContainer.Factory.Duplicate
{
    internal class PointerInfo
    {
        public int BufferSize;
        public long LastTimeStamp;
        public Point Position;
        public byte[] PtrShapeBuffer;
        public OutputDuplicatePointerShapeInformation ShapeInfo;
        public bool Visible;
        public int WhoUpdatedPositionLast;
    }
}