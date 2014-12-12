using MrWindows;
using MrWindows.KeyboardControl;
using SharpSenses;

namespace Sense.Behaviors {
    public class SwipeToControlTabBehavior : Behavior {
        public SwipeToControlTabBehavior(Windows windows, ICamera camera) : base(windows, camera) {}

        public override string Name {
            get { return "SwipeToControlTab"; }
        }

        public override void Activate() {
            Camera.Gestures.SwipeLeft += OnSwipeLeft;
            Camera.Gestures.SwipeRight += OnSwipeRight;
        }

        public override void Deactivate() {
            Camera.Gestures.SwipeLeft -= OnSwipeLeft;
            Camera.Gestures.SwipeRight -= OnSwipeRight;
        }

        private void OnSwipeRight(Hand h) {
            Windows.Keyboard.Type(VirtualKey.Control, VirtualKey.Shift, VirtualKey.Tab);
        }

        private void OnSwipeLeft(Hand h) {
            Windows.Keyboard.Type(VirtualKey.Control, VirtualKey.Tab);
        }
    }
}