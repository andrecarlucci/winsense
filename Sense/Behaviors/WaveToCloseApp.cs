using MrWindows;
using SharpSenses;
using SharpSenses.Gestures;

namespace Sense.Behaviors {
    public class WaveToCloseApp : Behavior {
        public WaveToCloseApp(Windows windows, ICamera camera) : base(windows, camera) {}

        public override string Name {
            get { return "WaveToCloseApp"; }
        }

        public override void Activate() {
            Camera.Gestures.Wave += GesturesOnWave;
        }

        private void GesturesOnWave(object sender, GestureEventArgs gestureEventArgs) {
            var p = Windows.CurrentWindow.GetForegroundProcess();
            p.Kill();
            //SendMessage("Close app " + p.ProcessName);
        }

        public override void Deactivate() {
            Camera.Gestures.Wave -= GesturesOnWave;
        }
    }
}