using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace CurrencyConvertor
{
    public partial class App : Application
    {
        private static Mutex _mutex = null;
        private const string AppName = "CurrencyConvertor";

        protected override void OnStartup(StartupEventArgs e)
        {
            // Ensure single instance
            bool createdNew;
            _mutex = new Mutex(true, AppName, out createdNew);

            if (!createdNew)
            {
                // App is already running, bring it to front
                BringExistingInstanceToFront();
                Current.Shutdown();
                return;
            }

            base.OnStartup(e);
        }

        private void BringExistingInstanceToFront()
        {
            Process current = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            {
                if (process.Id != current.Id)
                {
                    NativeMethods.SetForegroundWindow(process.MainWindowHandle);
                    NativeMethods.ShowWindow(process.MainWindowHandle, NativeMethods.SW_RESTORE);
                    break;
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _mutex?.ReleaseMutex();
            base.OnExit(e);
        }
    }
}