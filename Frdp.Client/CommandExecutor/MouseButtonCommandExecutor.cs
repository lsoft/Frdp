using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frdp.Client.Helpers;
using Frdp.Common;
using Frdp.Common.Command;

namespace Frdp.Client.CommandExecutor
{
    internal class MouseButtonCommandExecutor : ICommandExecutor<MouseButtonCommand>
    {
        public MouseButtonCommandExecutor(
            )
        {
        }

        public void Execute(
            MouseButtonCommand command
            )
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            switch (command.Action)
            {
                case MouseButtonCommand.MouseButtonCommandEnum.LeftButtonDown:
                    InvokeHelper.mouse_event(
                        InvokeHelper.MOUSEEVENTF_LEFTDOWN,
                        0,
                        0,
                        0,
                        0
                        );
                    break;
                case MouseButtonCommand.MouseButtonCommandEnum.LeftButtonUp:
                    InvokeHelper.mouse_event(
                        InvokeHelper.MOUSEEVENTF_LEFTUP,
                        0,
                        0,
                        0,
                        0
                        );
                    break;
                case MouseButtonCommand.MouseButtonCommandEnum.RightButtonDown:
                    InvokeHelper.mouse_event(
                        InvokeHelper.MOUSEEVENTF_RIGHTDOWN,
                        0,
                        0,
                        0,
                        0
                        );
                    break;
                case MouseButtonCommand.MouseButtonCommandEnum.RightButtonUp:
                    InvokeHelper.mouse_event(
                        InvokeHelper.MOUSEEVENTF_RIGHTUP,
                        0,
                        0,
                        0,
                        0
                        );
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
