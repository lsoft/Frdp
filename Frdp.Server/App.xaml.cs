using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Xml;
using Frdp.Common;
using Frdp.Common.Command;
using Frdp.Server.CompositionRoot;
using Frdp.Server.ViewModel;
using Frdp.Server.Wcf;
using Frdp.Server.Windows;
using Frdp.Wcf;
using Frdp.Wpf;
using Ninject;
using Ninject.Extensions.Factory;
using Application = System.Windows.Application;

namespace Frdp.Server
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Root _root = new Root();

        public App(
            )
        {
            _root.BindAll();
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _root.Get<MainWindow>();
            this.MainWindow = mainWindow;

            var rdpWindow = _root.Get<RdpWindow>();
            rdpWindow.Show();

            var fileWindow = _root.Get<FileWindow>();
            fileWindow.Show();

            mainWindow.Show();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            var listener = _root.Get<IListener>();
            listener.StopListen();

            _root.Dispose();
        }

    }
}
