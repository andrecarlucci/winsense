using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MrWindows;
using SharpSenses;

namespace Sense.Behaviors {
    public class LookDownForScrollDown : Behavior {
        public LookDownForScrollDown(Windows windows, ICamera camera) : base(windows, camera) {}

        public override string Name {
            get { return "LookDownForScrollDown"; }
        }

        public override void Activate() {
            Camera.Face.EyesDirectionChanged += ScrollDown;
        }

        private void ScrollDown(object sender, DirectionEventArgs directionEventArgs) {
            Windows.Mouse.ScrollVertically(-100);
        }

        public override void Deactivate() {
            Camera.Face.EyesDirectionChanged -= ScrollDown;            
        }
    }
}
