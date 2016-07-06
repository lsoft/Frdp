using System;
using System.Windows.Threading;
using Frdp.Client.FileTransfer;
using Frdp.Client.FileTransfer.Container;
using Frdp.Client.ViewModel;
using Frdp.Client.Windows;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;

namespace Frdp.Client.CompositionRoot
{
    public class NInjectFileTaskAdder : IFileTaskAdder
    {
        private readonly IResolutionRoot _root;
        private readonly Dispatcher _dispatcher;
        private readonly IFileTaskContainer _container;

        public NInjectFileTaskAdder(
            IResolutionRoot root,
            Dispatcher dispatcher,
            IFileTaskContainer container
            )
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            if (dispatcher == null)
            {
                throw new ArgumentNullException("dispatcher");
            }
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            _root = root;
            _dispatcher = dispatcher;
            _container = container;
        }

        public void AddTask(
            IFileTask task
            )
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            var success = _container.AddTask(task);

            if (success)
            {
                _dispatcher.BeginInvoke(
                    new Action(
                        () =>
                        {
                            var viewModel = _root.Get<FileViewModel>(
                                new ConstructorArgument(
                                    "fileTask",
                                    task
                                    )
                                );

                            var window = _root.Get<FileWindow>(
                                new ConstructorArgument(
                                    "viewModel",
                                    viewModel
                                    )
                                );

                            window.Show();
                        })
                    );
            }
        }
    }
}