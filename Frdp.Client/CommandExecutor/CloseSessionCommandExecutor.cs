using System;
using Frdp.Client.Connection;
using Frdp.Common.Command;

namespace Frdp.Client.CommandExecutor
{
    internal class CloseSessionCommandExecutor : ICommandExecutor<CloseSessionCommand>
    {
        private readonly IConnectionController _connectionController;

        public CloseSessionCommandExecutor(
            IConnectionController connectionController
            )
        {
            if (connectionController == null)
            {
                throw new ArgumentNullException("connectionController");
            }
            _connectionController = connectionController;
        }

        public void Execute(
            CloseSessionCommand command
            )
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            _connectionController.IsConnectionProceed = false;
        }
    }
}