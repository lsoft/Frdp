using System;
using System.Linq;
using Frdp.Client.Block;
using Frdp.Client.Block.Container.Factory;
using Frdp.Client.Block.Cutter;
using Frdp.Client.Block.Cutter.Cpp;
using Frdp.Client.Block.Cutter.Settings;
using Frdp.Client.Crc;
using Frdp.Client.Crc.Cpp;
using Frdp.Client.FileTransfer;
using Frdp.Client.FileTransfer.Container;
using Frdp.Client.NetworkWorker.FileChannel;
using Frdp.Common.Command;
using Ninject.Extensions.Factory;
using Ninject.Modules;

namespace Frdp.Client.CompositionRoot
{
    public sealed class BlockRelatedModule : NinjectModule
    {
        public override void Load()
        {

            Bind<IBlockSettingsFactory>()
                .ToFactory()
                .InSingletonScope()
                ;

            Bind<IBlockSettings>()
                .To<BlockSettings>()
                //NOT A SINGLETON!
                ;

            Bind<ICutterFactory>()
                .To<CppCutterFactory>()
                .InSingletonScope()
                ;

            Bind<ICrcCalculator>()
                .To<CppCrcCalculator>()
                .InSingletonScope()
                ;

            Bind<IBlockContainerFactory>()
                .To<BlockContainerFactory>()
                .InSingletonScope()
                ;

            Bind<IBlockDiffer>()
                .To<BlockDiffer>()
                .InSingletonScope()
                ;

        }
    }

    public sealed class FileTransferModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IFileTaskContainer>()
                .To<FileTaskContainer>()
                .InSingletonScope()
                ;

            Bind<IFileChannelWorker>()
                .To<FileChannelWorker>()
                .InSingletonScope()
                ;

            Bind<IFileTaskAdder>()
                .To<NInjectFileTaskAdder>()
                .InSingletonScope()
                ;
        }
    }

    public sealed class CommandModule : NinjectModule
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
