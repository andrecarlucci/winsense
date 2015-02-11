using MrWindows;

namespace Sense.Bob {
    public class ScrollActions : ISubject {
        private readonly Windows _windows;

        public ScrollActions(Windows windows) {
            _windows = windows;
        }

        public string GetAnswerFor(string sentence) {
            if (sentence.Contains("scroll left")) {
                _windows.Mouse.ScrollHorizontally(200);
            }
            else if (sentence.Contains("scroll right")) {
                _windows.Mouse.ScrollHorizontally(-200);
            }
            else if (sentence.Contains("scroll up")) {
                _windows.Mouse.ScrollVertically(-200);
            }
            else if (sentence.Contains("scroll down")) {
                _windows.Mouse.ScrollVertically(200);
            }
            return "";
        }
    }
}