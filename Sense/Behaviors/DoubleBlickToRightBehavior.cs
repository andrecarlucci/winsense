using System;
using MrWindows;
using MrWindows.KeyboardControl;
using SharpSenses;

namespace Sense.Behaviors {
    public class DoubleBlickToRightBehavior : Behavior {
        public DoubleBlickToRightBehavior(Windows windows, ICamera camera) : base(windows, camera) {}

        public override string Name {
            get { return "DoubleBlinkToRight"; }
        }

        public override void Activate() {
            Camera.Face.LeftEye.DoubleBlink += TypeRight;
        }

        private void TypeRight(object sender, EventArgs e) {
            Windows.Keyboard.Type(VirtualKey.Right);
        }

        public override void Deactivate() {
            Camera.Face.LeftEye.DoubleBlink -= TypeRight;
        }
    }
}