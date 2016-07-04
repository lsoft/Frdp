using System;
using System.Linq;
using Frdp.Common.Command;
using Ninject.Modules;

namespace Frdp.Client.CompositionRoot
{
    public class CommandModule : NinjectModule
    {
        public override void Load()
        {
            var typeSequence =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                where t.IsSubclassOf(typeof(BaseCommand))
                select t
                ;

            foreach (var t in typeSequence)
            {
                var name = BaseCommand.SalefyGetType(t);

                Bind<BaseCommand>()
                    .To(t)
                    //no singleton
                    .Named(name.ToLowerInvariant())
                ;
            }
        }
    }
}
