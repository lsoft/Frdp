using System;
using FrameworkExtensions.ThreadIsolation;
using Frdp.Client.Block;
using Frdp.Client.Block.Container;
using Frdp.Client.Block.Container.Factory;
using Frdp.Client.Channel.MainChannel;
using Frdp.Client.CommandExecutor;
using Frdp.Client.Connection;
using Frdp.Common;
using Frdp.Common.Settings;

namespace Frdp.Client.NetworkWorker.MainChannel
{
    internal class MainChannelWorker : IMainChannelWorker, IDisposable
    {
        private readonly IThreadIsolator _threadIsolator;
        private readonly IConnectionController _connectionController;
        private readonly IClientSettingsContainer _clientSettings;
        private readonly IBlockContainerFactory _blockContainerFactory;
        private readonly IBlockDiffer _blockDiffer;
        private readonly IMainChannelFactory _mainChannelFactory;
        private readonly ICommandExecutor _commandExecutor;
        private readonly ILogger _logger;

        private volatile bool _started = false;
        private volatile bool _disposed = false;
        
        private IThreadTaskInfo _tti;

        public MainChannelWorker(
            IThreadIsolator threadIsolator,
            IConnectionController connectionController,
            IClientSettingsContainer clientSettings,
            IBlockContainerFactory blockContainerFactory,
            IBlockDiffer blockDiffer,
            IMainChannelFactory mainChannelFactory,
            ICommandExecutor commandExecutor,
            ILogger logger
            )
        {
            if (threadIsolator == null)
            {
                throw new ArgumentNullException("threadIsolator");
            }
            if (connectionController == null)
            {
                throw new ArgumentNullException("connectionController");
            }
            if (clientSettings == null)
            {
                throw new ArgumentNullException("clientSettings");
            }
            if (blockContainerFactory == null)
            {
                throw new ArgumentNullException("blockContainerFactory");
            }
            if (blockDiffer == null)
            {
                throw new ArgumentNullException("blockDiffer");
            }
            if (mainChannelFactory == null)
            {
                throw new ArgumentNullException("mainChannelFactory");
            }
            if (commandExecutor == null)
            {
                throw new ArgumentNullException("commandExecutor");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            _threadIsolator = threadIsolator;
            _connectionController = connectionController;
            _clientSettings = clientSettings;
            _blockContainerFactory = blockContainerFactory;
            _blockDiffer = blockDiffer;
            _mainChannelFactory = mainChannelFactory;
            _commandExecutor = commandExecutor;
            _logger = logger;
        }

        public void AsyncStart(
            )
        {
            if (_started)
            {
                return;
            }

            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }

            _tti = _threadIsolator.RunThreadTyped(DoWork, false);

            _started = true;
        }

        public void SyncStop()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_tti != null)
                {
                    _tti.SyncDie();
                }

                _disposed = true;
            }
        }

        public void DoWork(
            IThreadDieCommander tdc
            )
        {
            if (tdc == null)
            {
                throw new ArgumentNullException("tdc");
            }

            while (!tdc.IsNeedToDie)
            {
                try
                {
                    if (_connectionController.IsConnectionProceed)
                    {
                        using (var channel = _mainChannelFactory.OpenChannel())
                        {
                            IBlockContainer left = null; //именно здесь (после внешнего while), чтобы после стоп-старта трансляции переслался полный кадр
                            while (!tdc.IsNeedToDie && _connectionController.IsConnectionProceed)
                            {
                                var right = _blockContainerFactory.CreateBlockContainer();

                                var diff = _blockDiffer.CalculateDiff(left, right);

                                var commandContainer = channel.SendFrame(
                                    diff
                                    );

                                left = right;

                                //обрабатываем команды
                                foreach (var cmd in commandContainer)
                                {
                                    try
                                    {
                                        _commandExecutor.Execute(cmd);
                                    }
                                    catch(Exception excp)
                                    {
                                        _logger.LogException(excp);
                                    }
                                }

                                if (tdc.WaitFor((int)_clientSettings.TimeoutBetweenFrames.TotalMilliseconds))
                                {
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        //если связь запрещена, просто ждем по секунде

                        if (tdc.WaitFor(1000))
                        {
                            return;
                        }
                    }
                }
                catch (Exception excp)
                {
                    _logger.LogException(excp);

                    //в случае ошибки ждем секунду (если ошибка будет перманентной, то без ожидания мы сожрем ЦП и зальем лог)
                    if (tdc.WaitFor(1000))
                    {
                        return;
                    }
                }
            }
        }
    }
}
