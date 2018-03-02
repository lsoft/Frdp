using System;
using System.Drawing;
using System.Windows.Forms;
using Frdp.Client.Helpers;
using Frdp.Client.ScreenInfo.Factory;
using Frdp.Common.Command;

namespace Frdp.Client.CommandExecutor
{
    internal class MousePositionCommandExecutor : ICommandExecutor<MousePositionCommand>
    {
        private readonly IScreenInfoFactory _screenInfoFactory;

        public MousePositionCommandExecutor(
            IScreenInfoFactory screenInfoFactory
            )
        {
            if (screenInfoFactory == null)
            {
                throw new ArgumentNullException("screenInfoFactory");
            }
            _screenInfoFactory = screenInfoFactory;
        }

        public void Execute(
            MousePositionCommand command
            )
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            using (var si = _screenInfoFactory.CreateScreenInfo())
            {
                var newX = si.Left + (int)(si.Width * command.RelativeX);
                var newY = si.Top + (int)(si.Height * command.RelativeY);

                var p = Cursor.Position;

                var oldX = p.X;
                var oldY = p.Y;

                if (oldX != newX || oldY != newY)
                {
                    InvokeHelper.mouse_event(
                        InvokeHelper.MOUSEEVENTF_MOVE | InvokeHelper.MOUSEEVENTF_ABSOLUTE,
                        (int)(command.RelativeX * 65536),
                        (int)(command.RelativeY * 65536),
                        0,
                        0
                        );

                    //Cursor.Position = new Point(
                    //    newX,
                    //    newY
                    //    );
                }
            }
        }
    }
}