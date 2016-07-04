using System;
using Frdp.Client.CommandContainer;
using Frdp.Common.Command;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;

namespace Frdp.Client.CompositionRoot
{
    public class NinjectCommandFactory : ICommandFactory
    {
        private readonly IResolutionRoot _root;

        public NinjectCommandFactory(
            IResolutionRoot root
            )
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            _root = root;
        }

        public BaseCommand CreateCommand(
            string type,
            object argument
            )
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            //argument allowed to be null

            return
                _root.Get<BaseCommand>(
                    type.ToLowerInvariant(),
                    new ConstructorArgument(
                        "node",
                        argument
                        )
                    );
        }
    }
}