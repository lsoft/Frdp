using System;

namespace Frdp.Server.Wcf
{
    public class GateListener : IListener
    {
        private readonly IListener _firstListener;
        private readonly IListener _secondListener;
        private readonly Frdp.Common.ILogger _logger;

        public GateListener(
            IListener firstListener,
            IListener secondListener,
            Frdp.Common.ILogger logger
            )
        {
            if (firstListener == null)
            {
                throw new ArgumentNullException("firstListener");
            }
            if (secondListener == null)
            {
                throw new ArgumentNullException("secondListener");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            _firstListener = firstListener;
            _secondListener = secondListener;
            _logger = logger;
        }

        public bool IsListening
        {
            get
            {
                return
                    _firstListener.IsListening && _secondListener.IsListening;
            }
        }

        public void StartListen()
        {
            try
            {
                _firstListener.StartListen();
            }
            catch (Exception excp)
            {
                _logger.LogException(excp);
            }

            try
            {
                _secondListener.StartListen();
            }
            catch (Exception excp)
            {
                _logger.LogException(excp);
            }
        }

        public void StopListen()
        {
            try
            {
                _firstListener.StopListen();
            }
            catch (Exception excp)
            {
                _logger.LogException(excp);
            }

            try
            {
                _secondListener.StopListen();
            }
            catch (Exception excp)
            {
                _logger.LogException(excp);
            }
        }
    }
}