using System;
using MrWindows;
using MrWindows.KeyboardControl;
using SharpSenses;
using SharpSenses.Gestures;

namespace Sense.Behaviors {
    public class SlideToControlTabBehavior : Behavior {
        public SlideToControlTabBehavior(Windows windows, ICamera camera) : base(windows, camera) {}

        public override string Name {
            get { return "SlideToControlTab"; }
        }

        public override void Activate() {
            Camera.Gestures.SlideLeft += OnSlideLeft;
            Camera.Gestures.SlideRight += OnSlideRight;
        }

        public override void Deactivate() {
            Camera.Gestures.SlideLeft -= OnSlideLeft;
            Camera.Gestures.SlideRight -= OnSlideRight;
        }

        private void OnSlideRight(object sender, GestureEventArgs args) {
            Windows.Keyboard.Type(VirtualKey.Control, VirtualKey.Shift, VirtualKey.Tab);
        }

        private void OnSlideLeft(object sender, GestureEventArgs args) {
            Windows.Keyboard.Type(VirtualKey.Control, VirtualKey.Tab);
        }
    }
}