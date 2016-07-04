namespace Frdp.Client.Suicider
{
    internal interface IApplicationSuicider
    {
        void AsyncStart();

        void SyncStop();

        void AsyncStop();
    }
}