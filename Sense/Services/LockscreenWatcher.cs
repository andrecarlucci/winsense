using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Dear;
using Sense.Storage;
using SharpSenses;
using XamlActions;

namespace Sense.Services {

    public class LockscreenWatcher {
        private readonly ICamera _camera;
        private readonly MrWindows _windows;
        private readonly Face _face;
        public static int ThresholdInSeconds = 5;
        private DateTime _lastSeen;

        public bool Enabled { get; set; }

        public LockscreenWatcher(ICamera camera, MrWindows windows) {
            _camera = camera;
            _windows = windows;
            _face = _camera.Face;
            Mediator.Default.Subscribe<LockscreenEnabledMessage>(this, m => Enabled = m.Enabled);
        }

        public void Start() {
            Task.Run(async () => {
                while (true) {
                    await Task.Delay(1000);
                    if (!Enabled || _face.IsVisible) {
                        _lastSeen = DateTime.Now;
                        continue;
                    }
                    Debug.WriteLine("[Warn] Lockscreen: " + (DateTime.Now - _lastSeen).TotalSeconds);
                    if ((DateTime.Now - _lastSeen).TotalSeconds > 5) {
                        _windows.LockWorkStation();
                    }
                }
            });
        }

        private bool IsRegisteredUser() {
            return _face.IsVisible && _face.UserId == WinSenseConfig.GetUser().Id;
        }
    }
}
