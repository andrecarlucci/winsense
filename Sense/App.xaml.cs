using System;
using System.Threading.Tasks;
using MrWindows;
using Sense.Lockscreen;
using Sense.Profiles;
using Sense.Services;
using Sense.Util;
using SharpSenses;
using SharpSenses.RealSense;
using XamlActions.DI;

namespace Sense {
    public partial class App {
        public static Windows MrWindows;
        public static ProcessMonitor ProcessMonitor;

        static App() {
            Item.DefaultNoiseThreshold = 2;
            var camera = Camera.Create(CameraKind.RealSense);
            MrWindows = new Windows();
            ProcessMonitor = new ProcessMonitor(MrWindows);
            ProcessMonitor.Start();
            ServiceLocator.Default.Register(camera);
            ServiceLocator.Default.Register(MrWindows);
            ServiceLocator.Default.Register(ProcessMonitor);
            ServiceLocator.Default.Register<IInputService>(typeof (InputService));

            var profileManager = new ProfileManager(camera, MrWindows, ProcessMonitor);
            ServiceLocator.Default.Register(profileManager);
            profileManager.Start();

            var findCamera = new StartCameraService(camera);
            findCamera.StartAsync().Wait();

            Task.Run(async () => {
                await Task.Delay(5000);
                var speechService = new SpeechService(camera);
                speechService.Start();
            });
            
            var unlocker = new Unlocker(new RealSenseCredentialPluginClient(), camera);
            unlocker.Start();
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