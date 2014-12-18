using System;
using MrWindows;
using MrWindows.KeyboardControl;
using SharpSenses;
using SharpSenses.Poses;

namespace Sense.Behaviors {
    public class JoinHandsToCloseAppBehavior : Behavior {

        private Pose _joinHands;

        public JoinHandsToCloseAppBehavior(Windows windows, ICamera camera) : base(windows, camera) {
            //_joinHands = PoseBuilder.Create()
            //    .ShouldBeNear(camera.LeftHand, camera.RightHand, 100)
            //    .Build();
        }

        public override string Name {
            get { return "JoinHandsToCloseAppBehavior"; }
        }

        public override void Activate() {
            _joinHands.Begin += JoinHandsOnBegin;
        }

        private void JoinHandsOnBegin(object sender, Pose.PoseEventArgs poseEventArgs) {
            Windows.CurrentWindow.GetForegroundProcess().Close();
            SendMessage("Close command");
        }

        public override void Deactivate() {
            _joinHands.Begin -= JoinHandsOnBegin;
        }
    }
}