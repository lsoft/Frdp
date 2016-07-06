using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Extensions.Compression;
using System.Text;
using System.Threading;
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
using Frdp.Client.Channel;
using Frdp.Client.Channel.MainChannel;
using Frdp.Client.CommandContainer;
using Frdp.Client.Crc;
using Frdp.Client.Crc.Cpp;
using Frdp.Client.FileTransfer;
using Frdp.Client.FileTransfer.Container;
using Frdp.Client.NetworkWorker.FileChannel;
using Frdp.Client.ScreenInfo.Factory;
using Frdp.Client.ScreenshotContainer.Factory;
using Frdp.Client.Suicider;
using Frdp.Client.ViewModel;
using Frdp.Client.Wcf;
using Frdp.Client.Wcf.MainChannel;
using Frdp.Client.Windows;
using Frdp.Common;
using Frdp.Common.Block.Diff;
using Frdp.Common.Settings;
using Frdp.Wcf;
using Frdp.Wcf.Endpoint;
using Ninject;
using Ninject.Extensions.Factory;

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

            _kernel
                .Bind<IFileTaskContainer>()
                .To<FileTaskContainer>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<IFileChannelWorker>()
                .To<FileChannelWorker>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<IFileTaskAdder>()
                .To<NInjectFileTaskAdder>()
                .InSingletonScope()
                ;

            var commandModule = new CommandModule();
            _kernel.Load(commandModule);

            var commandExecutorModule = new CommandExecutorModule();
            _kernel.Load(commandExecutorModule);

            var networkModule = new NetworkModule(
                _clac
                );
            _kernel.Load(networkModule);

            _logger = _kernel.Get<ILogger>();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            //var t = new Thread(
            //    () =>
            //    {
            //        Thread.Sleep(5000);

            //        var logger = _kernel.Get<ILogger>();
            //        var adder = _kernel.Get<IFileTaskAdder>();

            //        var ft = new FileTask(
            //            @"C:\projects\git\1 Frdp.7z",
            //            @"C:\projects\git\1 Frdp {received}.7z",
            //            163686287,
            //            true,
            //            true,
            //            logger
            //            );

            //        adder.AddTask(ft);
            //    });
            //t.Start();
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
