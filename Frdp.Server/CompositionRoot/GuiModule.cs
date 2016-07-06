using Frdp.Server.ViewModel;
using Frdp.Server.Windows;
using Ninject.Modules;

namespace Frdp.Server.CompositionRoot
{
    internal class GuiModule : NinjectModule
    {
        public override void Load()
        {
            BindMainWindow();
            BindRdpWindow();
            BindFileWindow();
        }

        private void BindFileWindow()
        {
            Bind<FileWindow>()
                .To<FileWindow>()
                .InSingletonScope()
                ;

            Bind<FileViewModel>()
                .To<FileViewModel>()
                .InSingletonScope()
                ;
        }

        private void BindRdpWindow()
        {
            Bind<RdpWindow>()
                .To<RdpWindow>()
                .InSingletonScope()
                ;

            Bind<RdpViewModel>()
                .To<RdpViewModel>()
                .InSingletonScope()
                ;
        }

        private void BindMainWindow()
        {
            Bind<MainWindow>()
                .To<MainWindow>()
                .InSingletonScope()
                ;

            Bind<MainViewModel>()
                .To<MainViewModel>()
                .InSingletonScope()
                ;
        }
    }
}