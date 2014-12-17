﻿using MrWindows;
using MrWindows.KeyboardControl;
using SharpSenses;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace Sense.Behaviors {
    public class JoinHandsToCloseAppBehavior : Behavior {

        private Pose _joinHands;

        public JoinHandsToCloseAppBehavior(Windows windows, ICamera camera) : base(windows, camera) {
            _joinHands = PoseBuilder.Create()
                .ShouldBeNear(camera.LeftHand.Index, camera.RightHand.Index, 100)
                .Build();
        }

        public override string Name {
            get { return "JoinHandsToCloseAppBehavior"; }
        }

        public override void Activate() {
            _joinHands.Begin += JoinHandsOnBegin;
        }

        private void JoinHandsOnBegin(object sender, Pose.PoseEventArgs poseEventArgs) {
            Windows.Keyboard
                .Press(VirtualKey.Alt)
                .Wait(100)
                .Press(VirtualKey.F4)
                .Wait(100)
                .Release(VirtualKey.F4)
                .Release(VirtualKey.Menu);
            SendMessage("Close command");
        }

        public override void Deactivate() {
            _joinHands.Begin -= JoinHandsOnBegin;
        }
    }
}