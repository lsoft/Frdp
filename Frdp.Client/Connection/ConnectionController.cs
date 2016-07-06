
namespace Frdp.Client.Connection
{
    public class ConnectionController : IConnectionController
    {
        public bool IsConnectionProceed
        {
            get;
            set;
        }

        public ConnectionController()
        {
            IsConnectionProceed = false;
        }

        public ConnectionController(
            bool isConnectionProceed
            )
        {
            IsConnectionProceed = isConnectionProceed;
        }
    }
}
