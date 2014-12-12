using System;
using MrWindows;
using Sense.Profiles;
using SharpSenses;

namespace Sense.Behaviors {
    public abstract class Behavior {
        public Windows Windows { get; set; }
        public ICamera Camera { get; set; }
        public abstract string Name { get; }

        protected object Sync = new object();

        protected Behavior(Windows windows, ICamera camera) {
            Windows = windows;
            Camera = camera;
        }
        
        public virtual void Activate() {}
        public virtual void Deactivate() { }
    }
}