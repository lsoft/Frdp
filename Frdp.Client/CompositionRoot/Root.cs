using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Extensions.Compression;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Xml;
using FrameworkExtensions.DateTimeProvider;
using FrameworkExtensions.ThreadIsolation;
using Frdp.Client.Block;
using Frdp.Client.Block.Container.Factory;
using Frdp.Client.Block.Cutter;
using Frdp.Client.Block.Cutter.Cpp;
using Frdp.Client.Block.Cutter.Settings;
using Frdp.Client.CommandContainer;
using Frdp.Client.ConnectionControl;
using Frdp.Client.Crc;
using Frdp.Client.Crc.Cpp;
using Frdp.Client.ScreenInfo.Factory;
using Frdp.Client.ScreenshotContainer.Factory;
using Frdp.Client.Suicider;
using Frdp.Client.ViewModel;
using Frdp.Client.Wcf;
using Frdp.Client.Window;
using Frdp.Common;
using Frdp.Common.Block.Diff;
using Frdp.Common.Settings;
using Frdp.Wcf;
using Frdp.Wcf.Endpoint;
using Ninject;
using Ninject.Extensions.Factory;
using IChannelFactory = Frdp.Client.Channel.IChannelFactory;

namespace Frdp.Client.CompositionRoot
{
    internal class Root : IDisposable
    {
        private readonly StandardKernel _kernel = new StandardKernel();

        private readonly CommandLineArgContainer _clac;

        private bool _disposed = false;

        private ILogger _logger;

        public T Get<T>()
        {
            return
                _kernel.Get<T>();
        }

        public Root(
            CommandLineArgContainer clac
            )
        {
            if (clac == null)
            {
                throw new ArgumentNullException("clac");
            }
            _clac = clac;
        }

        public void BindAll()
        {
            _kernel
                .Bind<ILogger>()
                .To<CombinedLogger>()
                .InSingletonScope()
                .WithConstructorArgument(
                    "journalLogMaxFileCount",
                    16u
                    )
                .WithConstructorArgument(
                    "isServiceMode",
                    false
                    )
                .WithConstructorArgument(
                    "isNeedToZipLogFiles",
                    false
                    )
                ;

            _kernel
                .Bind<IDateTimeProvider>()
                .To<DateTimeProvider>()
                .InSingletonScope()
                ;

            if (_clac.IsSuicideTimeoutExists)
            {
                _kernel
                    .Bind<IApplicationSuicider>()
                    .To<ApplicationSuicider>()
                    .InSingletonScope()
                    .WithConstructorArgument(
                        "aliveTimeout",
                        TimeSpan.FromMinutes(_clac.SuicideTimeoutMinutes) 
                        )
                    ;
            }
            else
            {
                _kernel
                    .Bind<IApplicationSuicider>()
                    .To<FakeApplicationSuicider>()
                    .InSingletonScope()
                    ;
            }

            _kernel
                .Bind<Application>()
                .ToConstant(Application.Current)
                .InSingletonScope()
                ;

            _kernel
                .Bind<IThreadIsolator>()
                .To<ThreadIsolator>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<IConnectionController>()
                .To<ConnectionController>()
                .InSingletonScope()
                .WithConstructorArgument(
                    "isConnectionProceed",
                    _clac.IsDefaultConnectionOnline
                    )
                ;

            _kernel
                .Bind<Dispatcher>()
                .ToConstant(App.Current.Dispatcher)
                .InSingletonScope()
                ;

            _kernel
                .Bind<MainWindow>()
                .To<MainWindow>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<MainViewModel>()
                .To<MainViewModel>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<IBlockSettings>()
                .To<BlockSettings>()
                //NOT A SINGLETON!
                .WithConstructorArgument(
                    "mask",
                    Convert.ToByte("11110000", 2)
                    )
                ;

            _kernel
                .Bind<IBlockSettingsFactory>()
                .ToFactory()
                .InSingletonScope()
                ;

            _kernel
                .Bind<IScreenInfoFactory>()
                //.To<ZenMachineScreenInfoFactory>()
                .To<PrimaryMonitorScreenInfoFactory>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<IScreenshotContainerFactory>()
                .To<DefaultScreenshotContainerFactory>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<ICutterFactory>()
                .To<CppCutterFactory>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<ICrcCalculator>()
                .To<CppCrcCalculator>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<IBlockContainerFactory>()
                .To<BlockContainerFactory>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<IBlockDiffer>()
                .To<BlockDiffer>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<CustomBinding>()
                .ToMethod(
                    c =>
                    {
                        var tcpTransport = new TcpTransportBindingElement();
                        tcpTransport.MaxReceivedMessageSize = 1024 * 1024 * 10;
                        tcpTransport.MaxBufferSize = 1024 * 1024 * 10;
                        tcpTransport.MaxBufferPoolSize = 1024 * 1024 * 10;
                        tcpTransport.TransferMode = TransferMode.Buffered;

                        var messageEncoder = new BinaryMessageEncodingBindingElement();
                        messageEncoder.ReaderQuotas = XmlDictionaryReaderQuotas.Max;


                        var compressionElement = new CompressionMessageEncodingBindingElement(
                            messageEncoder,
                            CompressionAlgorithm.GZip
                            );

                        var binding = new CustomBinding(
                            compressionElement,
                            tcpTransport
                            );

                        //binding.SendTimeout = new TimeSpan(0, 5, 0);
                        //binding.ReceiveTimeout = new TimeSpan(0, 5, 0);
                        //binding.OpenTimeout = new TimeSpan(0, 5, 0);
                        //binding.CloseTimeout = new TimeSpan(0, 5, 0);

                        return binding;
                    })
                .InSingletonScope()
                ;

            //_kernel
            //    .Bind<IChannel>()
            //    .To<WcfChannel>()
            //    //not a singleton!
            //    .WithConstructorArgument(
            //        "endpointAddress",
            //        "net.tcp://localhost:3310/Frdp"
            //        )
            //    ;

            _kernel
                .Bind<IChannelFactory>()
                .To<WcfChannelFactory>()
                .WhenInjectedExactlyInto<SuiciderChannelFactory>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<IChannelFactory>()
                .To<SuiciderChannelFactory>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<IEndpointContainer, IEndpointProvider>()
                .To<EndpointContainer>()
                .InSingletonScope()
                .WithConstructorArgument(
                    "endpointAddress",
                    _clac.IsConnectionAddressExists ? _clac.ConnectionAddress : "net.tcp://localhost:3310/Frdp")
                ;

            _kernel
                .Bind<IConnectivity>()
                .To<Connectivity>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<ICommandContainerFactory>()
                .To<CommandContainerFactory>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<ICommandFactory>()
                .To<NinjectCommandFactory>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<IClientSettingsProvider, IClientSettingsContainer>()
                .To<ClientSettings>()
                .InSingletonScope()
                ;

            var commandModule = new CommandModule();
            _kernel.Load(commandModule);

            var commandExecutorModule = new CommandExecutorModule();
            _kernel.Load(commandExecutorModule);

            _logger = _kernel.Get<ILogger>();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                _kernel.Dispose();
            }
        }

        private void CurrentDomainOnUnhandledException(
            object sender,
            UnhandledExceptionEventArgs unhandledExceptionEventArgs
            )
        {
            _logger.LogException(
                (Exception)unhandledExceptionEventArgs.ExceptionObject,
                "-- UNHANDLED EXCEPTION --"
                );
        }
    }
}
