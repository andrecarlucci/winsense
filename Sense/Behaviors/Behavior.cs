using MrWindows;
using Sense.Util;
using SharpSenses;
using XamlActions;

namespace Sense.Behaviors {
    public abstract class Behavior {

        public Windows Windows { get; set; }
        public ICamera Camera { get; set; }
        public abstract string Name { get; }

        protected object Sync = new object();

        protected void SendMessage(string message) {
            Mediator.Default.Publish(new NotifyIconMessage(message));
        }
             
        protected Behavior(Windows windows, ICamera camera) {
            Windows = windows;
            Camera = camera;
        }

        public abstract void Activate();
        public abstract void Deactivate();
    }
}