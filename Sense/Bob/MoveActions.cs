using MrWindows;
using MrWindows.KeyboardControl;

namespace Sense.Bob {
    public class MoveActions : ISubject {
        private readonly Windows _windows;

        public MoveActions(Windows windows) {
            _windows = windows;
        }

        public string GetAnswerFor(string sentence) {
            if (sentence.Contains("move left")) {
                _windows.Keyboard.Press(VirtualKey.Left);
            }
            else if (sentence.Contains("move right")) {
                _windows.Keyboard.Press(VirtualKey.Right);
            }
            else if (sentence.Contains("move up")) {
                _windows.Keyboard.Press(VirtualKey.Up);
            }
            else if (sentence.Contains("move down")) {
                _windows.Keyboard.Press(VirtualKey.Down);
            }
            return "";
        }
    }
}