using System;
using MrWindows;
using SharpSenses;

namespace Sense.Profiles {
    public abstract class Profile {
        protected Windows Windows { get; set; }
        protected ICamera Camera { get; set; }
        protected ProcessMonitor ProcessMonitor { get; set; }

        protected object _sync = new object();

        protected Profile(Windows windows, ICamera camera, ProcessMonitor processMonitor) {
            Windows = windows;
            Camera = camera;
            ProcessMonitor = processMonitor;
        }

        public abstract string Name { get; }
        public abstract void Config();
        public virtual void Deactivate() { }
        public virtual void DoIfActive(Action action) {
            if (ProfileManager.IsActive(this)) {
                action.Invoke();
            }
        }
    }
}