using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Dear;
using Sense.Lockscreen;
using Sense.Profiles;
using Sense.Services;
using Sense.Util;
using SharpSenses;
using SharpSenses.RealSense;
using XamlActions.DI;

namespace Sense {
    public partial class App {
        public static MrWindows MrWindows;
        public static ProcessMonitor ProcessMonitor;
        private static Process _speechProcess = new Process();

        static App() {
            Item.DefaultNoiseThreshold = 3;
            var camera = Camera.Create();
            camera.Speech.CurrentLanguage = SupportedLanguage.EnUS;
            MrWindows = new MrWindows();
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

            var locker = new LockscreenWatcher(camera, MrWindows);
            locker.Start();

            var unlocker = new Unlocker(new RealSenseCredentialPluginClient(), camera);
            unlocker.Start();

            //RunVoice();
        }

        public static void RunVoice() {
            try {
                Process[] pname = Process.GetProcessesByName("Sense.VoiceCommands");
                foreach (var process in pname) {
                    process.Kill();
                }
                _speechProcess.StartInfo.FileName = "Sense.VoiceCommands.exe";
                _speechProcess.StartInfo.UseShellExecute = false;
                _speechProcess.StartInfo.CreateNoWindow = true;
                _speechProcess.Start();
            }
            catch (Exception ex) {
                Debug.WriteLine("Speech: " + ex);
            }
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
            if (_speechProcess == null) {
                return;
            }
            try {
                _speechProcess.Kill();            
            }
            catch {
            }
            _speechProcess.SilentlyDispose();
        }
    }
    class MagicAttribute : Attribute { }
    class NoMagicAttribute : Attribute { }
}