using Dear;

namespace Sense.Bob {
    public class ClickActions : ISubject {
        private readonly MrWindows _windows;

        public ClickActions(MrWindows windows) {
            _windows = windows;
        }

        public string GetAnswerFor(string sentence) {
            if (sentence.StartsWith("click")) {
                _windows.Mouse.MouseLeftClick();
                return "";
            }
            return "";
        }

        public void OnOtherSubject() {
        }
    }
}