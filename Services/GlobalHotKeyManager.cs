using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace CurrencyConvertor
{
    public class GlobalHotKeyManager : IDisposable
    {
        private const int WM_HOTKEY = 0x0312;
        private const int HOTKEY_ID = 9000;
        private IntPtr _windowHandle;
        private HwndSource _source;
        private Action _callback;

        public void RegisterHotKey(Window window, ModifierKeys modifiers, Key key, Action callback)
        {
            var helper = new WindowInteropHelper(window);
            _windowHandle = helper.Handle;
            _callback = callback;

            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);

            var virtualKey = KeyInterop.VirtualKeyFromKey(key);
            var modifierFlags = GetModifierFlags(modifiers);

            if (!NativeMethods.RegisterHotKey(_windowHandle, HOTKEY_ID, modifierFlags, (uint)virtualKey))
            {
                throw new InvalidOperationException("Failed to register hotkey");
            }
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                _callback?.Invoke();
                handled = true;
            }
            return IntPtr.Zero;
        }

        private uint GetModifierFlags(ModifierKeys modifiers)
        {
            uint flags = 0;
            if (modifiers.HasFlag(ModifierKeys.Alt))
                flags |= 0x0001;
            if (modifiers.HasFlag(ModifierKeys.Control))
                flags |= 0x0002;
            if (modifiers.HasFlag(ModifierKeys.Shift))
                flags |= 0x0004;
            if (modifiers.HasFlag(ModifierKeys.Windows))
                flags |= 0x0008;
            return flags;
        }

        public void Dispose()
        {
            if (_windowHandle != IntPtr.Zero)
            {
                NativeMethods.UnregisterHotKey(_windowHandle, HOTKEY_ID);
                _source?.RemoveHook(HwndHook);
            }
        }
    }
}