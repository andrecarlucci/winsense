namespace Sense.Services {
    public class LockscreenEnabledMessage {
        public bool Enabled { get; set; }
        public LockscreenEnabledMessage(bool enabled) {
            Enabled = enabled;
        }
    }
}