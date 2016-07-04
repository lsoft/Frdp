
namespace Frdp.Client.ConnectionControl
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
