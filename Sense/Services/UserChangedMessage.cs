namespace Sense.Services {
    public class UserChangedMessage {
        public string Value { get; set; }

        public UserChangedMessage(string value) {
            Value = value;
        }
    }
}