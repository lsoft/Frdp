using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Frdp.Common;
using Frdp.Server.Applier;
using Frdp.Server.Wcf.MainChannel.Result;

namespace Frdp.Server.Wcf.MainChannel
{
    internal class WcfMainFactory : IInstanceProvider
    {
        private readonly IWcfResultFactory _resultFactory;
        private readonly IDiffApplier _diffApplier;
        private readonly ILogger _logger;

        public WcfMainFactory(
            IWcfResultFactory resultFactory,
            IDiffApplier diffApplier,
            ILogger logger
            )
        {
            if (resultFactory == null)
            {
                throw new ArgumentNullException("resultFactory");
            }
            if (diffApplier == null)
            {
                throw new ArgumentNullException("diffApplier");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _resultFactory = resultFactory;
            _diffApplier = diffApplier;
            _logger = logger;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return
                new WcfMainService(
                    _resultFactory,
                    _diffApplier,
                    _logger
                    );
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            //nothing to do
        }
    }
}