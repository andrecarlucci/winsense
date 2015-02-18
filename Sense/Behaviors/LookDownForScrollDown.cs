using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MrWindows;
using SharpSenses;
using MrWindows.KeyboardControl;

namespace Sense.Behaviors {
    public class DoubleBlinkToScrollDown : Behavior {
        public DoubleBlinkToScrollDown(Windows windows, ICamera camera) : base(windows, camera) {}

        public override string Name {
            get { return "DoubleBlinkToScrollDown"; }
        }

        public override void Activate() {
            Camera.Face.LeftEye.DoubleBlink += ScrollDown;
        }

        private void ScrollDown(object sender, EventArgs directionEventArgs) {
            Windows.Keyboard.Type(VirtualKey.Next);
            Debug.WriteLine("Double blink to scroll down");

        }

        public override void Deactivate() {
            Camera.Face.LeftEye.DoubleBlink -= ScrollDown;     
        }
    }
}
