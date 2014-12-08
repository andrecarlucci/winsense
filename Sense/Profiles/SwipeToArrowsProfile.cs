using System.Diagnostics;
using MrWindows;
using MrWindows.KeyboardControl;
using SharpSenses;

namespace Sense.Profiles {
    public class SwipeToArrowsProfile : Profile {
        public SwipeToArrowsProfile(Windows windows, ICamera camera, ProcessMonitor processMonitor) : base(windows, camera, processMonitor) {}

        public override string Name {
            get { return "SwipeToArrows"; }
        }

        public override void Config() {
            ConfigGestures();
            ProcessMonitor.ActiveProcessLoop += s => {
                if (s != "chrome") {
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
            Camera.Gestures.SwipeLeft += h => {
                //if(h.Side == Side.Left) return;
                //Debug.WriteLine("Left Arrow");
                //DoIfActive(() => Windows.Keyboard.Type(VirtualKey.Left));
            };
            Camera.Gestures.SwipeRight += h => {
                //if(h.Side == Side.Right) return;
                Debug.WriteLine("Right Arrow");
                DoIfActive(() => Windows.Keyboard.Type(VirtualKey.Right));
            };
        }
    }
}