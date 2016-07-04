using System;
using Frdp.Client.CommandExecutor;
using Frdp.Common.Command;
using Ninject;
using Ninject.Syntax;

namespace Frdp.Client.CompositionRoot
{
    internal class NinjectCommandExecutor : ICommandExecutor
    {
        private readonly IResolutionRoot _root;

        public NinjectCommandExecutor(
            IResolutionRoot root
            )
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }

            _root = root;
        }

        public void Execute(
            BaseCommand command
            )
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var type = typeof(ICommandExecutor<>).MakeGenericType(
                command.GetType()
                );

            dynamic executor = _root.Get(type);

            executor.Execute((dynamic)command);
        }
    }
}