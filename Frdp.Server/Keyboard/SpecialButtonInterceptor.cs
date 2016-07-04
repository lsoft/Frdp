using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;
using Frdp.Common;
using Frdp.Common.Command;
using Frdp.Server.ViewModel;

namespace Frdp.Server.Keyboard
{
    public class SpecialButtonInterceptor : ISpecialButtonInterceptor
    {
        private readonly ICommandContainer _commandContainer;

        private readonly InterceptKeys.LowLevelKeyboardProc _proc;
        private readonly IntPtr _keyboardHook;

        private volatile bool _started = false;

        private volatile bool _disposed = false;

        public SpecialButtonInterceptor(
            ICommandContainer commandContainer
            )
        {
            if (commandContainer == null)
            {
                throw new ArgumentNullException("commandContainer");
            }

            _commandContainer = commandContainer;

            _proc = HookCallback;
            try
            {
                _keyboardHook = InterceptKeys.SetHook(_proc);

                _started = true;
            }
            catch
            {
                DetachKeyboardHook();
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                if (_started)
                {
                    DetachKeyboardHook();
                }
            }
        }

        private enum WParamEnum
        {
            WM_KEYDOWN = 0x0100,
            WM_KEYUP = 0x0101,
            WM_SYSKEYDOWN = 0x0104,
            WM_SYSKEYUP = 0x0105
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var wp = (WParamEnum) wParam.ToInt32();

                bool alt = (Control.ModifierKeys & Keys.Alt) != 0;
                bool control = (Control.ModifierKeys & Keys.Control) != 0;

                int vkCode = Marshal.ReadInt32(lParam);
                var key = (Keys)vkCode;

                if (alt && key == Keys.Tab)
                {
                    KeyboardButtonCommand cmd = null;
                    if (wp == WParamEnum.WM_KEYDOWN || wp == WParamEnum.WM_SYSKEYDOWN)
                    {
                        //Debug.WriteLine("Hooked down " + key);

                        cmd = new KeyboardButtonCommand(
                            true,
                            Key.Tab
                            );

                    }
                    if (wp == WParamEnum.WM_KEYUP || wp == WParamEnum.WM_SYSKEYUP)
                    {
                        //Debug.WriteLine("Hooked up " + key);

                        cmd = new KeyboardButtonCommand(
                            false,
                            Key.Tab
                            );
                    }

                    if (cmd != null)
                    {
                        _commandContainer.AddCommand(cmd);
                    }

                    return
                        (IntPtr)1; // Handled.
                }

                if (key == Keys.LWin)
                {
                    KeyboardButtonCommand cmd = null;
                    if (wp == WParamEnum.WM_KEYDOWN || wp == WParamEnum.WM_SYSKEYDOWN)
                    {
                        cmd = new KeyboardButtonCommand(
                            true,
                            Key.LWin
                            );
                    }
                    if (wp == WParamEnum.WM_KEYUP || wp == WParamEnum.WM_SYSKEYUP)
                    {
                        cmd = new KeyboardButtonCommand(
                            false,
                            Key.LWin
                            );
                    }

                    if (cmd != null)
                    {
                        _commandContainer.AddCommand(cmd);
                    }

                    return
                        (IntPtr)1; // Handled.
                }

                if (key == Keys.RWin)
                {
                    KeyboardButtonCommand cmd = null;
                    if (wp == WParamEnum.WM_KEYDOWN || wp == WParamEnum.WM_SYSKEYDOWN)
                    {
                        cmd = new KeyboardButtonCommand(
                            true,
                            Key.RWin
                            );
                    }
                    if (wp == WParamEnum.WM_KEYUP || wp == WParamEnum.WM_SYSKEYUP)
                    {
                        cmd = new KeyboardButtonCommand(
                            false,
                            Key.RWin
                            );
                    }

                    if (cmd != null)
                    {
                        _commandContainer.AddCommand(cmd);
                    }

                    return
                        (IntPtr)1; // Handled.
                }
            }

            return
                InterceptKeys.CallNextHookEx(_keyboardHook, nCode, wParam, lParam);
        }

        /// <summary>
        /// Detach the keyboard hook; call during shutdown to prevent calls as we unload
        /// </summary>
        private void DetachKeyboardHook()
        {
            if (_keyboardHook != IntPtr.Zero)
            {
                InterceptKeys.UnhookWindowsHookEx(_keyboardHook);
            }
        }

    }
}
