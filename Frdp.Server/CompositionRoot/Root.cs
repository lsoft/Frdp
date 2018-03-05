using System;
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Frdp.Common;
using Frdp.Common.Command;
using Frdp.Common.Command.Container;
using Frdp.Common.Settings;
using Frdp.Server.AppController;
using Frdp.Server.Applier;
using Frdp.Server.Bitmap;
using Frdp.Server.Keyboard;
using Frdp.Server.Wcf;
using Frdp.Server.Wcf.FileChannel;
using Frdp.Server.Wcf.MainChannel;
using Frdp.Server.Wcf.MainChannel.Result;
using Frdp.Wcf;
using Frdp.Wcf.Endpoint;
using Frdp.Wpf;
using Ninject;
using Ninject.Extensions.Factory;

namespace Frdp.Server.CompositionRoot
{
    internal class Root : IDisposable
    {
        private readonly StandardKernel _kernel = new StandardKernel();

        private bool _disposed = false;
        
        private ILogger _logger;

        public T Get<T>()
        {
            return
                _kernel.Get<T>();
        }

        public void BindAll()
        {
            _kernel
                .Bind<Application>()
                .ToConstant(Application.Current)
                .InSingletonScope()
                ;

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
                .Bind<IClientSettingsProvider, IClientSettingsContainer>()
                .To<ClientSettings>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<DirectBitmapContainer>()
                .To<DirectBitmapContainer>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<IDiffApplier>()
                .To<ToBitmapDiffApplier>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<Dispatcher>()
                .ToConstant(App.Current.Dispatcher)
                .InSingletonScope()
                ;

            _kernel
                .Bind<ICommandContainer>()
                .To<CommandContainer>()
                .InSingletonScope()
                ;

            _kernel
                .Bind<ISpecialButtonInterceptor>()
                .To<SpecialButtonInterceptor>()
                //not a singleton
                ;

            _kernel
                .Bind<ISpecialButtonInterceptorFactory>()
                .ToFactory()
                .InSingletonScope()
                ;

            _kernel
                .Bind<IApplicationController>()
                .To<ApplicationController>()
                .InSingletonScope()
                ;

            var nm = new NetworkModule();
            _kernel.Load(nm);

            var guim = new GuiModule();
            _kernel.Load(guim);

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
