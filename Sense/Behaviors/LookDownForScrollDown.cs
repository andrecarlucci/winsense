using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MrWindows;
using SharpSenses;
using MrWindows.KeyboardControl;
using SharpSenses.Gestures;

namespace Sense.Behaviors {
    public class DoubleBlinkToScrollDown : Behavior {
        public DoubleBlinkToScrollDown(Windows windows, ICamera camera) : base(windows, camera) {}

        public override string Name {
            get { return "DoubleBlinkToScrollDown"; }
        }

        public override void Activate() {
            Camera.Face.LeftEye.DoubleBlink += ScrollDown;
            Camera.Face.LeftEye.Blink += ScrollUp;
            Camera.Face.RightEye.Blink += ScrollUp;
        }

        private void ScrollDown(object sender, EventArgs directionEventArgs) {
            Windows.Keyboard.Type(VirtualKey.Next);
        }

        private void ScrollUp(object sender, EventArgs e) {
            Windows.Keyboard.Type(VirtualKey.Prior);
        }

        public override void Deactivate() {
            Camera.Face.LeftEye.DoubleBlink -= ScrollDown;     
            Camera.Face.LeftEye.Blink -= ScrollUp;     
            Camera.Face.RightEye.Blink -= ScrollUp;     
        }
    }

    public class LookToAllDirectionsToArrows : Behavior {
        private bool _trackingEnabled;
        
        public LookToAllDirectionsToArrows(Windows windows, ICamera camera) : base(windows, camera) {}

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
