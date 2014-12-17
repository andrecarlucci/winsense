using System;
using MrWindows;
using MrWindows.KeyboardControl;
using SharpSenses;
using SharpSenses.Gestures;

namespace Sense.Behaviors {
    public class SlideToArrowsBehavior : Behavior {
        public SlideToArrowsBehavior(Windows windows, ICamera camera) : base(windows, camera) {}

        public override string Name {
            get { return "SlideToArrows"; }
        }

        public override void Activate() {
            Camera.Gestures.SlideLeft += OnSlideLeft;
            Camera.Gestures.SlideRight += OnSlideRight;
            Camera.Gestures.SlideUp += GesturesOnSlideUp;
            Camera.Gestures.SlideDown += GesturesOnSlideDown;
        }

        public override void Deactivate() {
            Camera.Gestures.SlideLeft -= OnSlideLeft;
            Camera.Gestures.SlideRight -= OnSlideRight;
            Camera.Gestures.SlideUp -= GesturesOnSlideUp;
            Camera.Gestures.SlideDown -= GesturesOnSlideDown;
        }

        private void OnSlideRight(object sender, GestureEventArgs args) {
            Windows.Keyboard.Type(VirtualKey.Right);
        }

        private void OnSlideLeft(object sender, GestureEventArgs args) {
            Windows.Keyboard.Type(VirtualKey.Left);
        }

        private void GesturesOnSlideUp(object sender, GestureEventArgs args) {
            Windows.Keyboard.Type(VirtualKey.Up);
        }

        private void GesturesOnSlideDown(object sender, GestureEventArgs gestureEventArgs) {
            Windows.Keyboard.Type(VirtualKey.Down);
        }
    }
}