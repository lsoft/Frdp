using System;
using System.ServiceModel;
using Frdp.Common;
using Frdp.Common.Block.Diff;
using Frdp.Server.Applier;
using Frdp.Server.Wcf.MainChannel.Result;
using Frdp.Wcf.Contract.MainChannel;

namespace Frdp.Server.Wcf.MainChannel
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class WcfMainService : IWcfMainChannel
    {
        private readonly IWcfResultFactory _resultFactory;
        private readonly IDiffApplier _diffApplier;
        private readonly ILogger _logger;
        
        public WcfMainService(
            IWcfResultFactory resultFactory,
            IDiffApplier diffApplier,
            ILogger logger
            )
        {
            if (resultFactory == null)
            {
                throw new ArgumentNullException("resultFactory");
            }
            if (diffApplier == null)
            {
                throw new ArgumentNullException("diffApplier");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _resultFactory = resultFactory;
            _diffApplier = diffApplier;
            _logger = logger;
        }

        public ServerCommands ExecuteRequest(Packet request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            ServerCommands result = null;

            try
            {
                var diffContainer = new DiffContainer(
                    request.ClientSettings.BlockWidth,
                    request.ClientSettings.BlockHeight,
                    request.ClientSettings.ImageWidth,
                    request.ClientSettings.ImageHeight,
                    request.ClientSettings.BlockCountHorizontal,
                    request.ClientSettings.BlockCountVertical,
                    request.ClientSettings.TotalBlockCount
                    );

                request.Diffs
                    .ConvertAll(j => new BlockDiff(j.X, j.Y, j.Data, 0, (uint)j.Data.Length))
                    .ForEach(j => diffContainer.AddDiff(j))
                    ;

                _diffApplier.Apply(
                    diffContainer
                    );

                result = _resultFactory.CreateServerCommands();

            }
            catch (Exception excp)
            {
                _logger.LogException(excp);
            }

            return
                result;
        }
    }
}