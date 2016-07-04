namespace Frdp.Client.ConnectionControl
{
    public interface IConnectivity
    {
        void AsyncStart(
            );

        void SyncStop(
            );
    }
}