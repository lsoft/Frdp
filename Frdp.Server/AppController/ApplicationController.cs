using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace Frdp.Server.AppController
{
    public class ApplicationController : IApplicationController
    {
        private readonly Application _application;
        private readonly Dispatcher _dispatcher;

        public ApplicationController(
            Application application,
            Dispatcher dispatcher
            )
        {
            if (application == null)
            {
                throw new ArgumentNullException("application");
            }
            if (dispatcher == null)
            {
                throw new ArgumentNullException("dispatcher");
            }
            _application = application;
            _dispatcher = dispatcher;
        }

        public void InitiateShutdown()
        {
            _dispatcher.BeginInvoke(
                new Action(
                    () =>
                    {
                        _application.Shutdown(0);
                    }));
        }
    }
}
