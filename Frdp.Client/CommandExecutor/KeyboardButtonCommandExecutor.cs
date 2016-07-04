using System;
using System.Windows.Input;
using Frdp.Client.Helpers;
using Frdp.Common.Command;

namespace Frdp.Client.CommandExecutor
{
    internal class KeyboardButtonCommandExecutor : ICommandExecutor<KeyboardButtonCommand>
    {
        public void Execute(
            KeyboardButtonCommand command
            )
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            switch (command.Key)
            {
                case Key.None:
                    break;
                case Key.Cancel:
                    break;
                case Key.Back:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.BACKSPACE);
                    break;
                case Key.Tab:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.TAB);
                    break;
                case Key.LineFeed:
                    break;
                case Key.Clear:
                    break;
                case Key.Return:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.ENTER);
                    break;
                case Key.Pause:
                    break;
                case Key.Capital:
                    break;
                case Key.KanaMode:
                    break;
                case Key.JunjaMode:
                    break;
                case Key.FinalMode:
                    break;
                case Key.HanjaMode:
                    break;
                case Key.Escape:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.ESC);
                    break;
                case Key.ImeConvert:
                    break;
                case Key.ImeNonConvert:
                    break;
                case Key.ImeAccept:
                    break;
                case Key.ImeModeChange:
                    break;
                case Key.Space:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.SPACE_BAR);
                    break;
                case Key.Prior:
                    break;
                case Key.Next:
                    break;
                case Key.End:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.END);
                    break;
                case Key.Home:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.HOME);
                    break;
                case Key.Left:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.LEFT);
                    break;
                case Key.Up:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.UP);
                    break;
                case Key.Right:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.RIGHT);
                    break;
                case Key.Down:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.DOWN);
                    break;
                case Key.Select:
                    break;
                case Key.Print:
                    break;
                case Key.Execute:
                    break;
                case Key.Snapshot:
                    break;
                case Key.Insert:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.INSERT);
                    break;
                case Key.Delete:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.DELETE);
                    break;
                case Key.Help:
                    break;
                case Key.D0:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_0);
                    break;
                case Key.D1:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_1);
                    break;
                case Key.D2:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_2);
                    break;
                case Key.D3:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_3);
                    break;
                case Key.D4:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_4);
                    break;
                case Key.D5:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_5);
                    break;
                case Key.D6:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_6);
                    break;
                case Key.D7:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_7);
                    break;
                case Key.D8:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_8);
                    break;
                case Key.D9:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_9);
                    break;
                case Key.A:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_A);
                    break;
                case Key.B:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_B);
                    break;
                case Key.C:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_C);
                    break;
                case Key.D:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_D);
                    break;
                case Key.E:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_E);
                    break;
                case Key.F:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_F);
                    break;
                case Key.G:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_G);
                    break;
                case Key.H:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_H);
                    break;
                case Key.I:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_I);
                    break;
                case Key.J:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_J);
                    break;
                case Key.K:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_K);
                    break;
                case Key.L:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_L);
                    break;
                case Key.M:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_M);
                    break;
                case Key.N:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_N);
                    break;
                case Key.O:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_O);
                    break;
                case Key.P:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_P);
                    break;
                case Key.Q:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_Q);
                    break;
                case Key.R:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_R);
                    break;
                case Key.S:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_S);
                    break;
                case Key.T:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_T);
                    break;
                case Key.U:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_U);
                    break;
                case Key.V:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_V);
                    break;
                case Key.W:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_W);
                    break;
                case Key.X:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_X);
                    break;
                case Key.Y:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_Y);
                    break;
                case Key.Z:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.KEY_Z);
                    break;
                case Key.LWin:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.LWIN);
                    break;
                case Key.RWin:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.RWIN);
                    break;
                case Key.Apps:
                    break;
                case Key.Sleep:
                    break;
                case Key.NumPad0:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.NUMPAD0);
                    break;
                case Key.NumPad1:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.NUMPAD1);
                    break;
                case Key.NumPad2:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.NUMPAD2);
                    break;
                case Key.NumPad3:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.NUMPAD3);
                    break;
                case Key.NumPad4:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.NUMPAD4);
                    break;
                case Key.NumPad5:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.NUMPAD5);
                    break;
                case Key.NumPad6:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.NUMPAD6);
                    break;
                case Key.NumPad7:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.NUMPAD7);
                    break;
                case Key.NumPad8:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.NUMPAD8);
                    break;
                case Key.NumPad9:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.NUMPAD9);
                    break;
                case Key.Multiply:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.MULTIPLY);
                    break;
                case Key.Add:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.ADD);
                    break;
                case Key.Separator:
                    break;
                case Key.Subtract:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.SUBTRACT);
                    break;
                case Key.Decimal:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.DECIMAL);
                    break;
                case Key.Divide:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.DIVIDE);
                    break;
                case Key.F1:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F1);
                    break;
                case Key.F2:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F2);
                    break;
                case Key.F3:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F3);
                    break;
                case Key.F4:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F4);
                    break;
                case Key.F5:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F5);
                    break;
                case Key.F6:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F6);
                    break;
                case Key.F7:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F7);
                    break;
                case Key.F8:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F8);
                    break;
                case Key.F9:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F9);
                    break;
                case Key.F10:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F10);
                    break;
                case Key.F11:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F11);
                    break;
                case Key.F12:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F12);
                    break;
                case Key.F13:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F13);
                    break;
                case Key.F14:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F14);
                    break;
                case Key.F15:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F15);
                    break;
                case Key.F16:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F16);
                    break;
                case Key.F17:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F17);
                    break;
                case Key.F18:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F18);
                    break;
                case Key.F19:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F19);
                    break;
                case Key.F20:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F20);
                    break;
                case Key.F21:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F21);
                    break;
                case Key.F22:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F22);
                    break;
                case Key.F23:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F23);
                    break;
                case Key.F24:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.F24);
                    break;
                case Key.NumLock:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.NUMLOCK);
                    break;
                case Key.Scroll:
                    break;
                case Key.LeftShift:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.LSHIFT);
                    break;
                case Key.RightShift:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.RSHIFT);
                    break;
                case Key.LeftCtrl:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.LCONTROL);
                    break;
                case Key.RightCtrl:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.RCONTROL);
                    break;
                case Key.LeftAlt:
                case Key.RightAlt:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.ALT);
                    break;
                case Key.BrowserBack:
                    break;
                case Key.BrowserForward:
                    break;
                case Key.BrowserRefresh:
                    break;
                case Key.BrowserStop:
                    break;
                case Key.BrowserSearch:
                    break;
                case Key.BrowserFavorites:
                    break;
                case Key.BrowserHome:
                    break;
                case Key.VolumeMute:
                    break;
                case Key.VolumeDown:
                    break;
                case Key.VolumeUp:
                    break;
                case Key.MediaNextTrack:
                    break;
                case Key.MediaPreviousTrack:
                    break;
                case Key.MediaStop:
                    break;
                case Key.MediaPlayPause:
                    break;
                case Key.LaunchMail:
                    break;
                case Key.SelectMedia:
                    break;
                case Key.LaunchApplication1:
                    break;
                case Key.LaunchApplication2:
                    break;
                case Key.Oem1:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.OEM_1);
                    break;
                case Key.OemPlus:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.OEM_PLUS);
                    break;
                case Key.OemComma:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.OEM_COMMA);
                    break;
                case Key.OemMinus:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.OEM_MINUS);
                    break;
                case Key.OemPeriod:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.OEM_PERIOD);
                    break;
                case Key.Oem2:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.OEM_2);
                    break;
                case Key.Oem3:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.OEM_3);
                    break;
                case Key.AbntC1:
                    break;
                case Key.AbntC2:
                    break;
                case Key.Oem4:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.OEM_4);
                    break;
                case Key.Oem5:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.OEM_5);
                    break;
                case Key.Oem6:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.OEM_6);
                    break;
                case Key.Oem7:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.OEM_7);
                    break;
                case Key.Oem8:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.OEM_8);
                    break;
                case Key.Oem102:
                    SendKey(command.IsKeyDown, SendInputProxy.KeyCode.OEM_102);
                    break;
                case Key.ImeProcessed:
                    break;
                case Key.System:
                    break;
                case Key.OemAttn:
                    break;
                case Key.OemFinish:
                    break;
                case Key.OemCopy:
                    break;
                case Key.OemAuto:
                    break;
                case Key.OemEnlw:
                    break;
                case Key.OemBackTab:
                    break;
                case Key.Attn:
                    break;
                case Key.CrSel:
                    break;
                case Key.ExSel:
                    break;
                case Key.EraseEof:
                    break;
                case Key.Play:
                    break;
                case Key.Zoom:
                    break;
                case Key.NoName:
                    break;
                case Key.Pa1:
                    break;
                case Key.OemClear:
                    break;
                case Key.DeadCharProcessed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SendKey(
            bool isKeyDown,
            SendInputProxy.KeyCode code
            )
        {
            if (isKeyDown)
            {
                SendInputProxy.SendKeyDown(code);
            }
            else
            {
                SendInputProxy.SendKeyUp(code);
            }
        }
    }
}