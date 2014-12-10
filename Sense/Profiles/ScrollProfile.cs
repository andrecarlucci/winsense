using System.Collections.Generic;
using System.Drawing;
using MrWindows;
using Sense.Util;
using SharpSenses;
using SharpSenses.Gestures;
using SharpSenses.Poses;

namespace Sense.Profiles {
    public class ScrollProfile : Profile {
        private List<Profile> _lastActiveProfiles;

        public ScrollProfile(Windows windows, ICamera camera, ProcessMonitor processMonitor) : base(windows, camera, processMonitor) {}

        public override string Name {
            get { return "Scroll"; }
        }

        public override void Config() {
            var screenSize = Windows.GetActiveScreenSize();
            var cameraToScreenMapper = new CameraToScreenMapper(
                screenSize.Width,
                screenSize.Height,
                Camera.LeftHand);
            cameraToScreenMapper.Moved += LeftHandMoved;

            var closeBothHands = new PoseBuilder().ShouldBe(Camera.LeftHand, State.Closed)
                .ShouldBe(Camera.RightHand, State.Closed)
                .ShouldBe(Camera.LeftHand, State.Visible)
                .ShouldBe(Camera.RightHand, State.Visible)
                .Build("bothHandsClosed");

            closeBothHands.Begin += n => {
                _lastActiveProfiles = ProfileManager.ActiveProfiles;
                ProfileManager.Activate(this);
            };
            closeBothHands.End += n => DoIfActive(() => {
                ProfileManager.Deactivate(this);
                ProfileManager.Activate(_lastActiveProfiles.ToArray());                    
            });
        }

        public void LeftHandMoved(Point pointFrom, Point pointTo) {
            if (!ProfileManager.IsActive(this)) {
                return;
            }
            var direction = DirectionHelper.GetDirection(pointFrom, pointTo);
            if (direction == Direction.Left || direction == Direction.Right) {
                Windows.Mouse.ScrollHorizontally(pointTo.X - pointFrom.X);
            }
            else {
                Windows.Mouse.ScrollVertically(pointTo.Y - pointFrom.Y);
            }
        }

        public override void Deactivate() {
            Windows.Mouse.MouseLeftUp();
        }
    }
}