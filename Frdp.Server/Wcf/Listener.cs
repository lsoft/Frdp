using System;
using System.Collections;
using System.Linq;
using Castle.Core.Internal;
using Castle.Core.Logging;
using Frdp.Wcf;
using Frdp.Wcf.Endpoint;

namespace Frdp.Server.Wcf
{
    public class Listener : IListener
    {
        private readonly Func<IWCFListener> _listenerFactory;
        private readonly IEndpointProvider _endpointContainer;

        private readonly object _locker = new object();

        private IWCFListener _listener;


        public Listener(
            Func<IWCFListener> listenerFactory,
            IEndpointProvider endpointContainer
            )
        {
            if (listenerFactory == null)
            {
                throw new ArgumentNullException("listenerFactory");
            }
            if (endpointContainer == null)
            {
                throw new ArgumentNullException("endpointContainer");
            }

            _listenerFactory = listenerFactory;
            _endpointContainer = endpointContainer;
        }

        public bool IsListening
        {
            get
            {
                return
                    _listener != null;
            }
        }

        public void StartListen(
            )
        {
            lock (_locker)
            {
                if (_listener != null)
                {
                    return;
                }

                _listener = _listenerFactory();

                var endpoint = _endpointContainer.GetEndpointAddress();

                _listener.StartListen(endpoint);
            }
        }

        public void StopListen(
            )
        {
            lock (_locker)
            {
                if (_listener == null)
                {
                    return;
                }

                _listener.StopListen();
                _listener = null;
            }
        }
    }
}
