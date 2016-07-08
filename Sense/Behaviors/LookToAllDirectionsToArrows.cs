using System;
using Dear;
using Dear.KeyboardControl;
using SharpSenses;
using SharpSenses.Gestures;

namespace Sense.Behaviors {
    public class LookToAllDirectionsToArrows : Behavior {
        private bool _trackingEnabled;
        
        public LookToAllDirectionsToArrows(MrWindows windows, ICamera camera) : base(windows, camera) {}

        public override string Name {
            get {
                return "LookToAllDirectionsToArrows";
            }
        }

        public override void Activate() {
            _trackingEnabled = false;
            Camera.Face.LeftEye.DoubleBlink += EnableEyesTracking;
            Camera.Face.EyesDirectionChanged += FireArrows;
        }

        private void FireArrows(object sender, DirectionEventArgs e) {
            if (!_trackingEnabled) {
                return;
            }
            switch (e.NewDirection) {
                case Direction.Up:
                    Windows.Keyboard.Type(VirtualKey.Up);
                    return;
                case Direction.Down:
                    Windows.Keyboard.Type(VirtualKey.Down);
                    return;
                case Direction.Left:
                    Windows.Keyboard.Type(VirtualKey.Left);
                    return;
                case Direction.Right:
                    Windows.Keyboard.Type(VirtualKey.Down);
                    return;
            }
        }

        private void EnableEyesTracking(object sender, EventArgs e) {
            _trackingEnabled = !_trackingEnabled;
        }

        public override void Deactivate() {
            Camera.Face.LeftEye.DoubleBlink -= EnableEyesTracking;
            Camera.Face.EyesDirectionChanged -= FireArrows;
        }
    }
}