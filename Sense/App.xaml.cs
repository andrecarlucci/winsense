using System;
using MrWindows;
using Sense.Services;
using SharpSenses;
using SharpSenses.RealSense;
using XamlActions.DI;

namespace Sense {
    public partial class App {
        public static Windows MrWindows;
        public static ProcessMonitor ProcessMonitor;

        static App() {
            var camera = Camera.Create(CameraKind.RealSense);
            MrWindows = new Windows();
            ProcessMonitor = new ProcessMonitor(MrWindows);
            ProcessMonitor.Start();
            ServiceLocator.Default.Register(camera);
            ServiceLocator.Default.Register(MrWindows);
            ServiceLocator.Default.Register(ProcessMonitor);

            var findCamera = new StartCameraService(camera);
            findCamera.StartAsync();
        }

        public App() {
            DispatcherUnhandledException += CurrentOnExit;
            Exit += CurrentOnExit;
        }

        private static void CurrentOnExit(object sender, EventArgs exitEventArgs) {
            if (Sense.MainWindow.TaskbarIcon != null) {
                Sense.MainWindow.TaskbarIcon.Icon = null;
                Sense.MainWindow.TaskbarIcon.Dispose();
            }
        }
    }
    class MagicAttribute : Attribute { }
    class NoMagicAttribute : Attribute { }
}