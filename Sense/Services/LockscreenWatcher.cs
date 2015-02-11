using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MrWindows;
using Sense.Storage;
using SharpSenses;

namespace Sense.Services {

    public class FaceTracker {
        private readonly ICamera _camera;
        public int CurrentUserId { get; private set; }
        public DateTime LastSeen { get; private set; }

        public FaceTracker(ICamera camera) {
            _camera = camera;
        }

        public void Start() {
            _camera.Face.FaceRecognized += FaceOnFaceRecognized;
            _camera.Face.Visible += (s, a) => {
                LastSeen = DateTime.Now;
                Debug.WriteLine("Face visible");
            };
            _camera.Face.NotVisible += (s, a) => {
                LastSeen = DateTime.Now;
                Debug.WriteLine("Face not visible");
            };
        }

        public void RecognizeCurrentUser() {
            CurrentUserId = 0;
            _camera.Face.RecognizeFace();
        }

        private void FaceOnFaceRecognized(object sender, FaceRecognizedEventArgs args) {
            if (args.UserId == CurrentUserId) {
                return;
            }
            CurrentUserId = args.UserId;
            Config.Default.Set(ConfigKeys.UserId, args.UserId);
        }
    }

    public class LockscreenWatcher {
        private readonly FaceTracker _faceTracker;
        private readonly Windows _windows;
        public static int ThresholdInSeconds = 5;

        public bool Active { get; set;}
        public bool Enabled { get; set; }

        public LockscreenWatcher(FaceTracker faceTracker, Windows windows) {
            _faceTracker = faceTracker;
            _windows = windows;
        }

        public void Start() {
            Task.Run(async () => {
                while (true) {
                    await Task.Delay(1000);
                    if (!Active) {
                        continue;
                    }
                    if (_faceTracker.CurrentUserId < 0 && DateTime.Now - _faceTracker.LastSeen > TimeSpan.FromSeconds(ThresholdInSeconds)) {
                        _windows.LockWorkStation();
                    }
                }
            });
        }        
    }
}
