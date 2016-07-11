using System.ServiceModel;

namespace Frdp.Wcf.Contract.FileChannel
{
    [ServiceContract]
    public interface IWcfFileChannel
    {
        [OperationContract]
        byte[] GetData(
            string filepath, 
            long offset, 
            int length
            );

        [OperationContract]
        void AppendData(
            string filepath,
            bool forceToCreateFolder,
            byte[] data
            );
    }
}