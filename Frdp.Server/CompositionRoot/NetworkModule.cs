using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using Frdp.Server.Wcf;
using Frdp.Server.Wcf.FileChannel;
using Frdp.Server.Wcf.MainChannel;
using Frdp.Server.Wcf.MainChannel.Result;
using Frdp.Wcf.Endpoint;
using Ninject;
using Ninject.Modules;

namespace Frdp.Server.CompositionRoot
{
    internal class NetworkModule : NinjectModule
    {
        private const string WcfMainListenerKey = "WcfMainListener";
        private const string WcfFileListenerKey = "WcfFileListener";

        public override void Load()
        {
            BindMainChannel();
            BindFileChannel();
            BindCommon();
        }

        private void BindMainChannel()
        {
            Bind<IServiceBehavior>()
                .To<WcfMainBehaviour>()
                .WhenInjectedExactlyInto<WcfMainListener>()
                .InSingletonScope()
                ;

            Bind<IWCFListener>()
                .To<WcfMainListener>()
                //not a singleton
                .Named(WcfMainListenerKey)
                ;

            Bind<IListener>()
                .To<Listener>()
                .WhenTypeAndConstructorArgumentNamed(typeof(GateListener), "firstListener")
                .InSingletonScope()
                .WithConstructorArgument(
                    "listenerFactory",
                    c => new Func<IWCFListener>(() => c.Kernel.Get<IWCFListener>(WcfMainListenerKey))
                    )
                ;
        }

        private void BindFileChannel()
        {
            Bind<IServiceBehavior>()
                .To<WcfFileBehaviour>()
                .WhenInjectedExactlyInto<WcfFileListener>()
                .InSingletonScope()
                ;

            Bind<IWCFListener>()
                .To<WcfFileListener>()
                //not a singleton
                .Named(WcfFileListenerKey)
                ;

            Bind<IListener>()
                .To<Listener>()
                .WhenTypeAndConstructorArgumentNamed(typeof(GateListener), "secondListener")
                .InSingletonScope()
                .WithConstructorArgument(
                    "listenerFactory",
                    c => new Func<IWCFListener>(() => c.Kernel.Get<IWCFListener>(WcfFileListenerKey))
                    )
                ;
        }

        private void BindCommon(
            )
        {
            Bind<IListener>()
                .To<GateListener>()
                .InSingletonScope()
                ;

            Bind<IWcfResultFactory>()
                .To<WcfResultFactory>()
                .InSingletonScope()
                ;

            Bind<IEndpointContainer, IEndpointProvider>()
                .To<EndpointContainer>()
                .InSingletonScope()
                ;
        }
    }
}
