using System;
using System.Diagnostics;
using MrWindows;
using MrWindows.KeyboardControl;
using SharpSenses;

namespace Sense.Profiles {
    public class SwipeToControlTabProfile : Profile {
        public SwipeToControlTabProfile(Windows windows, ICamera camera, ProcessMonitor processMonitor)
            : base(windows, camera, processMonitor) {}

        public override string Name {
            get { return "SwipeToControlTab"; }
        }

        public override void Config() {
            ConfigGestures();
            ProcessMonitor.ActiveProcessLoop += s => {
                if (s == "chrome") {
                    if (ProfileManager.IsEmpty()) {
                        ProfileManager.Activate(this);
                    }
                }
                else {
                    ProfileManager.Deactivate(this);
                }
            };
        }

        protected void ConfigGestures() {
            Camera.Gestures.SwipeLeft +=
                h => DoIfActive(() => {
                    //if (h.Side == Side.Left) return;
                    Debug.WriteLine("Control+Shift+Tab");
                    Windows.Keyboard.Type(VirtualKey.Control, VirtualKey.Shift, VirtualKey.Tab);
                });
            Camera.Gestures.SwipeRight +=
                h => DoIfActive(() => {
                    //if (h.Side == Side.Right) return;
                    Debug.WriteLine("Control+Tab");
                    Windows.Keyboard.Type(VirtualKey.Control, VirtualKey.Tab);
                });
        }
    }
}