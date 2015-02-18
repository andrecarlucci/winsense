using System;
using System.Diagnostics;
using System.Drawing;
using MrWindows;
using Sense.Util;
using SharpSenses;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace Sense.Behaviors {
    public class HandToMouseBehavior : Behavior {

        private CameraToScreenMapper _cameraToScreenMapper;
        private bool _scrolling;
        private Pose _closeBothHands;

        public HandToMouseBehavior(Windows windows, ICamera camera)
            : base(windows, camera) {
            _closeBothHands = new PoseBuilder()
                .ShouldBe(Camera.LeftHand, State.Closed)
                .ShouldBe(Camera.RightHand, State.Closed)
                .ShouldBe(Camera.LeftHand, State.Visible)
                .ShouldBe(Camera.RightHand, State.Visible)
                .Build("bothHandsClosed");
        }

        public override string Name {
            get { return "HandToMouse"; }
        }

        public override void Activate() {
            var screenSize = Windows.GetActiveScreenSize();
            _cameraToScreenMapper = new CameraToScreenMapper(
                screenSize.Width,
                screenSize.Height,
                Camera.LeftHand);

            _cameraToScreenMapper.Moved += CameraToScreenMapperOnMoved;

            Camera.Gestures.MoveForward += OnMoveForward;
            
            Camera.RightHand.Closed += OnRightHandOnClosed;
            Camera.RightHand.Opened += OnRightHandOnOpened;
            
            Camera.RightHand.Visible += OnRightHandVisible;
            Camera.RightHand.NotVisible += OnRightHandNotVisible;
            
            _closeBothHands.Begin += CloseBothHandsOnBegin;
            _closeBothHands.End += CloseBothHandsOnEnd;
        }

        private void CloseBothHandsOnBegin(object sender, Pose.PoseEventArgs poseEventArgs) {
            _scrolling = true;
        }

        private void CloseBothHandsOnEnd(object sender, Pose.PoseEventArgs poseEventArgs) {
            _scrolling = false;
        }

        private void CameraToScreenMapperOnMoved(Point p1, Point p2) {
            if (_scrolling) {
                var direction = DirectionHelper.GetDirection(p1, p2);
                if (direction == Direction.Left || direction == Direction.Right) {
                    Windows.Mouse.ScrollHorizontally(p2.X - p1.X);
                }
                else {
                    Windows.Mouse.ScrollVertically(p2.Y - p1.Y);
                }
                return;
            }
            Windows.Mouse.MoveCursor(p2.X, p2.Y);
        }

        public override void Deactivate() {
            _cameraToScreenMapper.Moved -= CameraToScreenMapperOnMoved;
            Camera.Gestures.MoveForward -= OnMoveForward;
            Camera.RightHand.Closed -= OnRightHandOnClosed;
            Camera.RightHand.Opened -= OnRightHandOnOpened;
            Camera.RightHand.Visible -= OnRightHandVisible;
            Camera.RightHand.NotVisible -= OnRightHandNotVisible;
            _cameraToScreenMapper.Dispose();
            _closeBothHands.Begin -= CloseBothHandsOnBegin;
            _closeBothHands.End -= CloseBothHandsOnEnd;
            Windows.Mouse.MouseLeftUp();
        }

        private void OnRightHandNotVisible(object sender, EventArgs args) {
            Debug.WriteLine("NotVisible");
            Windows.Mouse.MouseLeftUp();
            _scrolling = false;
        }

        private void OnRightHandVisible(object sender, EventArgs args) {
            Debug.WriteLine("Visible");
            
            Windows.Mouse.MouseLeftUp();
            _scrolling = false;
        }


        private void OnRightHandOnOpened(object sender, EventArgs args) {
            Debug.WriteLine("Up");
            Windows.Mouse.MouseLeftUp();
            _scrolling = false;
        }

        private void OnRightHandOnClosed(object sender, EventArgs args) {
            Debug.WriteLine("Down");
            Windows.Mouse.MouseLeftDown();
        }

        private void OnMoveForward(object sender, EventArgs args) {
            //Windows.Mouse.MouseLeftClick();
        }
    }
}