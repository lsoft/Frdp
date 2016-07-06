using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Frdp.Common;

namespace Frdp.Server.Wcf.FileChannel
{
    internal class WcfFileFactory : IInstanceProvider
    {
        private readonly ILogger _logger;

        public WcfFileFactory(
            ILogger logger
            )
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _logger = logger;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return
                new WcfFileService(
                    _logger
                    );
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            //nothing to do
        }
    }
}