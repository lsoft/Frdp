using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using System.Windows.Threading;
using FrameworkExtensions.DateTimeProvider;
using FrameworkExtensions.ThreadIsolation;
using Action = OpenCvSharp.Util.Action;

namespace Frdp.Client.Suicider
{
    internal class ApplicationSuicider : IApplicationSuicider, IDisposable
    {
        private const int TimeoutMsec = 1000;

        private readonly IThreadIsolator _threadIsolator;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly Application _currentApplication;
        private readonly TimeSpan _aliveTimeout;

        private volatile bool _started = false;
        private volatile bool _disposed = false;
        
        private IThreadTaskInfo _tti;
        
        private DateTime _startTime;

        public ApplicationSuicider(
            IThreadIsolator threadIsolator,
            IDateTimeProvider dateTimeProvider,
            Application currentApplication,
            TimeSpan aliveTimeout
            )
        {
            if (threadIsolator == null)
            {
                throw new ArgumentNullException("threadIsolator");
            }
            if (dateTimeProvider == null)
            {
                throw new ArgumentNullException("dateTimeProvider");
            }
            if (currentApplication == null)
            {
                throw new ArgumentNullException("currentApplication");
            }
            _threadIsolator = threadIsolator;
            _dateTimeProvider = dateTimeProvider;
            _currentApplication = currentApplication;
            _aliveTimeout = aliveTimeout;
        }

        public void AsyncStart()
        {
            if (_started)
            {
                return;
            }

            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }

            _startTime = _dateTimeProvider.GetCurrentTime();
            _tti = _threadIsolator.RunThreadTyped(DoWork, false);

            _started = true;
        }

        public void SyncStop()
        {
            SyncDispose();
        }

        public void AsyncStop()
        {
            AsyncDispose();
        }

        public void Dispose()
        {
            SyncDispose();
        }

        public void DoWork(
            IThreadDieCommander tdc
            )
        {
            if (tdc == null)
            {
                throw new ArgumentNullException("tdc");
            }

            while (!tdc.WaitFor(TimeoutMsec))
            {
                var current = _dateTimeProvider.GetCurrentTime();
                var diff = current - _startTime;

                if (diff >= _aliveTimeout)
                {
                    //таймаут вышел
                    _currentApplication.Dispatcher.BeginInvoke(
                        new Action(
                            () => _currentApplication.Shutdown()
                            )
                        );
                }
            }
        }

        private void AsyncDispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                if (_tti != null)
                {
                    _tti.AsyncDie();
                }
            }
        }

        private void SyncDispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                if (_tti != null)
                {
                    _tti.SyncDie();
                }
            }
        }
    }
}
