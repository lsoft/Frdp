using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frdp.Client.ScreenInfo.Factory;
using Frdp.Client.ScreenshotContainer.Factory;
using Ninject.Modules;

namespace Frdp.Client.CompositionRoot
{
    internal class ScreenshotModule : NinjectModule
    {
        private readonly CommandLineArgContainer _clac;

        public ScreenshotModule(
            CommandLineArgContainer clac
            )
        {
            if (clac == null)
            {
                throw new ArgumentNullException("clac");
            }
            _clac = clac;
        }

        public override void Load()
        {
            Type ftype;

            var v = Environment.OSVersion.Version;
            if (v.Major > 6
                || (v.Major == 6 && v.Minor >= 2)
                )
            {
                //win8 or newer

                ftype = typeof(DesktopDuplicationScreenshotContainerFactory);
            }
            else
            {
                //win7 or older

                ftype = typeof (DefaultScreenshotContainerFactory);


                Bind<IScreenInfoFactory>()
                    //.To<ZenMachineScreenInfoFactory>()
                    .To<PrimaryMonitorScreenInfoFactory>()
                    .InSingletonScope()
                    ;
            }

            if (_clac.IsDebugInfoEnabled)
            {
                Bind<IScreenshotContainerFactory>()
                    .To(ftype)
                    .WhenInjectedExactlyInto<DebugScreenshotContainerFactory>()
                    .InSingletonScope()
                    ;

                Bind<IScreenshotContainerFactory>()
                    .To<DebugScreenshotContainerFactory>()
                    .InSingletonScope()
                    ;
            }
            else
            {
                Bind<IScreenshotContainerFactory>()
                    .To(ftype)
                    .InSingletonScope()
                    ;
            }
        }
    }
}
