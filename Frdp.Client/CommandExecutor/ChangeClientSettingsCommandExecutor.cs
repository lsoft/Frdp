using System;
using Frdp.Client.FileTransfer.Container;
using Frdp.Common.Command;
using Frdp.Common.Settings;

namespace Frdp.Client.CommandExecutor
{
    internal class ChangeClientSettingsCommandExecutor : ICommandExecutor<ChangeClientSettingsCommand>
    {
        private readonly IClientSettingsProvider _originalClientSettings;

        public ChangeClientSettingsCommandExecutor(
            IClientSettingsProvider originalClientSettings
            )
        {
            if (originalClientSettings == null)
            {
                throw new ArgumentNullException("originalClientSettings");
            }
            _originalClientSettings = originalClientSettings;
        }

        public void Execute(
            ChangeClientSettingsCommand command
            )
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            _originalClientSettings.SetBlockSize(
                command.BlockWidth,
                command.BlockHeight
                );

            _originalClientSettings.SetScaleFactor(
                command.ScaleFactorX,
                command.ScaleFactorY
                );

            _originalClientSettings.SetTimeoutBetweenFrames(
                command.TimeoutBetweenFrames
                );

            _originalClientSettings.SetMask(
                command.Mask
                );
        }
    }
}