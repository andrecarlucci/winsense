using MrWindows;

namespace Sense.Bob {
    public class ClickActions : ISubject {
        private readonly Windows _windows;

        public ClickActions(Windows windows) {
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