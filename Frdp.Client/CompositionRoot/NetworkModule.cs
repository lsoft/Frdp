using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Extensions.Compression;
using System.Text;
using System.Xml;
using Frdp.Client.Channel.FileChannel;
using Frdp.Client.Channel.MainChannel;
using Frdp.Client.Connection;
using Frdp.Client.NetworkWorker.MainChannel;
using Frdp.Client.Wcf;
using Frdp.Client.Wcf.FileChannel;
using Frdp.Client.Wcf.MainChannel;
using Frdp.Wcf.Endpoint;
using Ninject.Modules;

namespace Frdp.Client.CompositionRoot
{
    internal class NetworkModule : NinjectModule
    {
        private readonly CommandLineArgContainer _clac;

        public NetworkModule(
            CommandLineArgContainer clac
            )
        {
            if (clac == null)
            {
                throw new ArgumentNullException("clac");
            }
            _clac = clac;
        }

        public override void Load()
        {
            BindMainChannel();
            BindFileChannel();
            BindCommon();
        }

        private void BindCommon()
        {

            Bind<IEndpointContainer, IEndpointProvider>()
                .To<EndpointContainer>()
                .InSingletonScope()
                .WithConstructorArgument(
                    "endpointAddress",
                    _clac.IsConnectionAddressExists ? _clac.ConnectionAddress : "net.tcp://localhost:3310/Frdp"
                    )
                ;

            Bind<IMainChannelWorker>()
                .To<MainChannelWorker>()
                .InSingletonScope()
                ;

            Bind<IConnectionController>()
                .To<ConnectionController>()
                .InSingletonScope()
                .WithConstructorArgument(
                    "isConnectionProceed",
                    _clac.IsDefaultConnectionOnline
                    )
                ;
        }

        private void BindFileChannel()
        {
            Bind<IBindingProvider>()
                .To<FileBindingProvider>()
                .WhenInjectedExactlyInto<WcfFileChannelFactory>()
                .InSingletonScope()
                ;

            Bind<IFileChannelFactory>()
                .To<WcfFileChannelFactory>()
                .InSingletonScope()
                ;
        }

        private void BindMainChannel()
        {
            Bind<IBindingProvider>()
                .To<MainBindingProvider>()
                .WhenInjectedExactlyInto<WcfMainChannelFactory>()
                .InSingletonScope()
                ;

            Bind<IMainChannelFactory>()
                .To<WcfMainChannelFactory>()
                .WhenInjectedExactlyInto<SuiciderMainChannelFactory>()
                .InSingletonScope()
                ;

            Bind<IMainChannelFactory>()
                .To<SuiciderMainChannelFactory>()
                .InSingletonScope()
                ;
        }
    }
}
