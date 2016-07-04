using System;
using System.Windows.Input;
using System.Windows.Threading;
using Frdp.Client.ConnectionControl;
using Frdp.Wcf;
using Frdp.Wcf.Endpoint;
using Frdp.Wpf;

namespace Frdp.Client.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IEndpointContainer _endpointContainer;
        private readonly IConnectionController _connectionController;

        public string Endpoint
        {
            get
            {
                return
                    _endpointContainer.EndpointAddress;
            }

            set
            {
                _endpointContainer.EndpointAddress = value;
            }
        }

        public bool IsConnectionProceed
        {
            get
            {
                return
                    _connectionController.IsConnectionProceed;
            }

            set
            {
                _connectionController.IsConnectionProceed = value;
            }
        }

        private ICommand _startConnectionCommand;
        public ICommand StartConnectionCommand
        {
            get
            {
                if (_startConnectionCommand == null)
                {
                    _startConnectionCommand = new RelayCommand(
                        j =>
                        {
                            _connectionController.IsConnectionProceed = true;

                            OnPropertyChanged(string.Empty);
                            OnCommandInvalidate();
                        },
                        j => true
                        );
                }

                return
                    _startConnectionCommand;
            }
        }

        private ICommand _stopConnectionCommand;
        public ICommand StopConnectionCommand
        {
            get
            {
                if (_stopConnectionCommand == null)
                {
                    _stopConnectionCommand = new RelayCommand(
                        j =>
                        {
                            _connectionController.IsConnectionProceed = false;

                            OnPropertyChanged(string.Empty);
                            OnCommandInvalidate();
                        },
                        j => true
                        );
                }

                return
                    _stopConnectionCommand;
            }
        }


        public MainViewModel(
            Dispatcher dispatcher,
            IEndpointContainer endpointContainer,
            IConnectionController connectionController
            ) : base(dispatcher)
        {
            if (connectionController == null)
            {
                throw new ArgumentNullException("connectionController");
            }

            _endpointContainer = endpointContainer;
            _connectionController = connectionController;
        }
    }
}
