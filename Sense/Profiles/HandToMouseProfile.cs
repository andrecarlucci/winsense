using MrWindows;
using Sense.Util;
using SharpSenses;

namespace Sense.Profiles {
    public class HandToMouseProfile : Profile {

        public HandToMouseProfile(Windows windows, ICamera camera, ProcessMonitor processMonitor) : base(windows, camera, processMonitor) {}

        public override string Name {
            get { return "HandToMouse"; }
        }
       

        public override void Config() {
            var screenSize = Windows.GetActiveScreenSize();
            var cameraToScreenMapper = new CameraToScreenMapper(
                screenSize.Width,
                screenSize.Height,
                Camera.LeftHand);
            cameraToScreenMapper.Moved += (p1, p2) => DoIfActive(() => Windows.Mouse.MoveCursor(p2.X, p2.Y));

            Camera.Poses.PeaceBegin += h => {
                if (h.Side != Side.Left || ProcessMonitor.CurrentProcess != "chrome") {
                    return;
                }
                if (ProfileManager.IsActive(this)) {
                    ProfileManager.Deactivate(this);
                    return;
                }
                ProfileManager.Activate(this);
            };

            Camera.Gestures.MoveForward += h => DoIfActive(() => Windows.Mouse.MouseLeftClick());

            Camera.RightHand.Closed += () => DoIfActive(() => Windows.Mouse.MouseLeftDown());
            Camera.RightHand.Opened += () => DoIfActive(() => Windows.Mouse.MouseLeftUp());
            
            Camera.RightHand.Visible += () => DoIfActive(() => Windows.Mouse.MouseLeftUp());
            Camera.RightHand.NotVisible += () => DoIfActive(() => Windows.Mouse.MouseLeftUp());
        }

        public override void Deactivate() {
            Windows.Mouse.MouseLeftUp();
            ProfileManager.Deactivate(this);
        }
    }
}