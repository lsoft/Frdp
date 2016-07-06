using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Frdp.Common;
using Frdp.Server.Applier;
using Frdp.Server.Wcf.MainChannel.Result;

namespace Frdp.Server.Wcf.MainChannel
{
    internal class WcfMainBehaviour : IServiceBehavior
    {
        private readonly IWcfResultFactory _resultFactory;
        private readonly IDiffApplier _diffApplier;
        private readonly ILogger _logger;

        public WcfMainBehaviour(
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

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var cdb in serviceHostBase.ChannelDispatchers)
            {
                var cd = cdb as ChannelDispatcher;
                if (cd != null)
                {
                    foreach (var ed in cd.Endpoints)
                    {
                        ed.DispatchRuntime.InstanceProvider = new WcfMainFactory(
                            _resultFactory,
                            _diffApplier,
                            _logger
                            );
                    }
                }
            }
        }

        public void Validate(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters
            )
        {
        }
    }
}