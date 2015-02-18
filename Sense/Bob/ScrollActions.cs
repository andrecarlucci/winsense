using System.Threading;
using MrWindows;

namespace Sense.Bob {
    public class ScrollActions : ISubject {
        private readonly Windows _windows;

        public ScrollActions(Windows windows) {
            _windows = windows;
        }

        public string GetAnswerFor(string sentence) {
            const string scroll = "scroll|screw|screwed|straight|stroke";
            const string left = "left|let|let's|lift";
            const string right = "right|rights";
            const string up = "up|much|lunch|duck|of|book|it";
            const string down = "down|now";
            if (sentence.HasWordsInSequence(scroll, left)) {
                _windows.Mouse.ScrollHorizontally(200);
                Thread.Sleep(300);
                _windows.Mouse.ScrollHorizontally(200);
            }
            else if (sentence.HasWordsInSequence(scroll, right)) {
                _windows.Mouse.ScrollHorizontally(-200);
                Thread.Sleep(300);
                _windows.Mouse.ScrollHorizontally(-200);
            }
            else if (sentence.HasWordsInSequence(scroll, up)) {
                _windows.Mouse.ScrollVertically(200);
                Thread.Sleep(300);
                _windows.Mouse.ScrollVertically(200);
            }
            else if (sentence.HasWordsInSequence(scroll, down)) {
                _windows.Mouse.ScrollVertically(-200);
                Thread.Sleep(300);
                _windows.Mouse.ScrollVertically(-200);
            }
            else {
                return "";
            }
            return "ok";
        }

        public void OnOtherSubject() {
            
        }
    }
}