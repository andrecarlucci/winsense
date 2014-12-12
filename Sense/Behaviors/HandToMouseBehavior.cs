using System.Drawing;
using MrWindows;
using Sense.Util;
using SharpSenses;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace Sense.Behaviors {
    public class HandToMouseBehavior : Behavior {

        private CameraToScreenMapper _cameraToScreenMapper;
        private bool _scrolling = false;
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

            _cameraToScreenMapper.Moved += OnLeftHandMoved;
            Camera.Gestures.MoveForward += OnMoveForward;
            Camera.RightHand.Closed += OnRightHandOnClosed;
            Camera.RightHand.Opened += OnRightHandOnOpened;
            Camera.RightHand.Visible += OnRightHandVisible;
            Camera.RightHand.NotVisible += OnRightHandNotVisible;
            _closeBothHands.Begin += OnCloseBothHandsBegin;
            _closeBothHands.End += OnCloseBothHandsEnd;
        }

        private void OnCloseBothHandsEnd(string n) {
            _scrolling = false;
        }

        private void OnCloseBothHandsBegin(string n) {
            _scrolling = true;
        }

        public override void Deactivate() {
            _cameraToScreenMapper.Moved -= OnLeftHandMoved;
            Camera.Gestures.MoveForward -= OnMoveForward;
            Camera.RightHand.Closed -= OnRightHandOnClosed;
            Camera.RightHand.Opened -= OnRightHandOnOpened;
            Camera.RightHand.Visible -= OnRightHandVisible;
            Camera.RightHand.NotVisible -= OnRightHandNotVisible;
            _cameraToScreenMapper.Dispose();
            _closeBothHands.Begin -= OnCloseBothHandsBegin;
            _closeBothHands.End -= OnCloseBothHandsEnd;
            Windows.Mouse.MouseLeftUp();
        }

        private void OnRightHandNotVisible() {
            Windows.Mouse.MouseLeftUp();
            _scrolling = false;
        }

        private void OnRightHandVisible() {
            Windows.Mouse.MouseLeftUp();
            _scrolling = false;
        }

        private void OnRightHandOnOpened() {
            Windows.Mouse.MouseLeftUp();
            _scrolling = false;
        }

        private void OnRightHandOnClosed() {
            Windows.Mouse.MouseLeftDown();
        }

        private void OnMoveForward(Hand h) {
            Windows.Mouse.MouseLeftClick();
        }

        private void OnLeftHandMoved(Point p1, Point p2) {
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
    }
}