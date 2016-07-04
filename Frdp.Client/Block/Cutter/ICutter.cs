using OpenCvSharp;

namespace Frdp.Client.Block.Cutter
{
    public interface ICutter
    {
        void DoCut(
            Mat m1
            );
    }
}