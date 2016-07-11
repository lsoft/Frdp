using System;
using System.Collections.Generic;
using System.Threading;
using FrameworkExtensions.ThreadIsolation;
using Frdp.Client.Channel.FileChannel;
using Frdp.Client.Connection;
using Frdp.Client.FileTransfer;
using Frdp.Client.FileTransfer.Container;
using Frdp.Common;

namespace Frdp.Client.NetworkWorker.FileChannel
{
    public class FileChannelWorker : IFileChannelWorker, IDisposable
    {
        private readonly IThreadIsolator _threadIsolator;
        private readonly IConnectionController _connectionController;
        private readonly IFileTaskContainer _fileTaskContainer;
        private readonly IFileChannelFactory _fileChannelFactory;
        private readonly ILogger _logger;


        private IThreadTaskInfo _tti;

        private volatile bool _started = false;
        private volatile bool _disposed = false;

        public FileChannelWorker(
            IThreadIsolator threadIsolator,
            IConnectionController connectionController,
            IFileTaskContainer fileTaskContainer,
            IFileChannelFactory fileChannelFactory,
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
            if (fileTaskContainer == null)
            {
                throw new ArgumentNullException("fileTaskContainer");
            }
            if (fileChannelFactory == null)
            {
                throw new ArgumentNullException("fileChannelFactory");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _threadIsolator = threadIsolator;
            _connectionController = connectionController;
            _fileTaskContainer = fileTaskContainer;
            _fileChannelFactory = fileChannelFactory;
            _logger = logger;
        }


        public void AsyncStart()
        {
            if (_started)
            {
                return;
            }
            if(_disposed)
            {
                return;
            }

            _tti = _threadIsolator.RunThreadTyped(DoWork, false);

            _started = true;
        }

        public void SyncStop()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                if (_tti != null)
                {
                    //останавливаем доставку файла
                    _tti.SyncDie();
                    _tti = null;
                }
            }
        }

        private void DoWork(IThreadDieCommander tdc)
        {
            if (tdc == null)
            {
                throw new ArgumentNullException("tdc");
            }

            while (!tdc.IsNeedToDie)
            {
                try
                {

                    List<IFileTask> alives = null;
                    while (!tdc.WaitFor(1000) && (alives == null || alives.Count == 0))
                    {
                        if (_connectionController.IsConnectionProceed)
                        {
                            alives = _fileTaskContainer.GetAliveTasks();
                        }
                    }

                    using (var channel = _fileChannelFactory.OpenChannel())
                    {
                        do
                        {
                            if (alives != null)
                            {
                                foreach (var task in alives)
                                {
                                    task.ProcessOneIteration(
                                        channel
                                        );

                                    if (tdc.IsNeedToDie)
                                    {
                                        return;
                                    }
                                }
                            }

                            alives = _fileTaskContainer.GetAliveTasks();
                        }
                        while (_connectionController.IsConnectionProceed && alives != null && alives.Count > 0);

                    }
                }
                catch (Exception excp)
                {
                    _logger.LogException(excp);

                    Thread.Sleep(1000);
                }
            }
        }
    }
}