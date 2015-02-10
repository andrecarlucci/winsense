using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MrWindows;
using SharpSenses;
using MrWindows.KeyboardControl;

namespace Sense.Behaviors {
    public class DoubleBlinkForScrollDown : Behavior {
        public DoubleBlinkForScrollDown(Windows windows, ICamera camera) : base(windows, camera) {}

        public override string Name {
            get { return "DoubleBlinkForScrollDown"; }
        }

        public override void Activate() {
            Camera.Face.LeftEye.DoubleBlink += ScrollDown;
        }

        private void ScrollDown(object sender, EventArgs directionEventArgs) {
            Windows.Keyboard.Type(VirtualKey.Next);
            //Windows.Mouse.ScrollVertically(-200);
        }

        public override void Deactivate() {
            Camera.Face.LeftEye.DoubleBlink -= ScrollDown;     
        }
    }
}
