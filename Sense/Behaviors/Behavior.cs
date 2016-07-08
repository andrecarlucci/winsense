using Dear;
using Sense.Util;
using SharpSenses;
using XamlActions;

namespace Sense.Behaviors {
    public abstract class Behavior {

        public MrWindows Windows { get; set; }
        public ICamera Camera { get; set; }
        public abstract string Name { get; }

        protected object Sync = new object();

        protected void SendMessage(string message) {
            Mediator.Default.Publish(new NotifyIconMessage(message));
        }
             
        protected Behavior(MrWindows windows, ICamera camera) {
            Windows = windows;
            Camera = camera;
        }

        public abstract void Activate();
        public abstract void Deactivate();
    }
}