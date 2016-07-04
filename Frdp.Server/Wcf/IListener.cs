namespace Frdp.Server.Wcf
{
    public interface IListener
    {
        bool IsListening
        {
            get;
        }

        void StartListen(
            );

        void StopListen(
            );
    }
}