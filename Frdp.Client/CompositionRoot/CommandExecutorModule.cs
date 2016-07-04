using System;
using System.Linq;
using Frdp.Client.CommandExecutor;
using Frdp.Common.Command;
using Ninject.Modules;

namespace Frdp.Client.CompositionRoot
{
    public class CommandExecutorModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICommandExecutor>()
                .To<NinjectCommandExecutor>()
                .InSingletonScope()
                ;

            var sequence =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                let interfaces = t.GetInterfaces().Where(j => j.IsGenericType && j.GetGenericTypeDefinition() == typeof (ICommandExecutor<>)).ToList()
                where interfaces.Count == 1
                let ourInterface = interfaces[0]
                let interfaceGenericArguments = ourInterface.GetGenericArguments()
                let ourInterfaceGenericArgument = interfaceGenericArguments[0]
                let bindingName = BaseCommand.SalefyGetType(ourInterfaceGenericArgument)
                select new
                {
                    BindingName = bindingName.ToLowerInvariant(),
                    ExecutorInterface = ourInterface,
                    Executor = t
                }
                ;

            foreach (var s in sequence)
            {
                Bind(s.ExecutorInterface)
                    .To(s.Executor)
                    .InSingletonScope()
                    .Named(s.BindingName)
                    ;
            }
        }
    }
}