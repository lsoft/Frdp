using System;
using System.Linq;
using System.Windows;
using Frdp.Client.CommandExecutor;
using Frdp.Client.CompositionRoot;
using Frdp.Client.ConnectionControl;
using Frdp.Client.Suicider;
using Frdp.Client.Window;
using Frdp.Common.Command;
using Application = System.Windows.Application;

namespace Frdp.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Root _root;

        public App(
            )
        {
            var clac = new CommandLineArgContainer();

            _root = new Root(
                clac
                );

            //foreach (var f in Directory.GetFiles(".", "*.bmp"))
            //{
            //    File.Delete(f);
            //}

            _root.BindAll();
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var connectivity = _root.Get<IConnectivity>();
            connectivity.AsyncStart();

            var suicider = _root.Get<IApplicationSuicider>();
            suicider.AsyncStart();

            var mainWindow = _root.Get<MainWindow>();
            this.MainWindow = mainWindow;

            mainWindow.Show();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            var connectivity = _root.Get<IConnectivity>();
            connectivity.SyncStop();

            _root.Dispose();
        }
    }
}

