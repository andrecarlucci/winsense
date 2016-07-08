
using Dear;
using Dear.KeyboardControl;

namespace Sense.Bob {
    public class MoveActions : ISubject {
        private readonly MrWindows _windows;

        public MoveActions(MrWindows windows) {
            _windows = windows;
        }

        public string GetAnswerFor(string sentence) {
            const string press = "press|breasts|breast|rest";
            const string left = "left|let|let's|lift|lunch";
            const string right = "right|rights";
            const string up = "up|much|lunch|duck|of|book|look";
            const string down = "down|now";

            if (sentence.HasWordsInSequence(press, left)) {
                _windows.Keyboard.Press(VirtualKey.Left);                
            }
            else if (sentence.HasWordsInSequence(press, right)) {
                _windows.Keyboard.Press(VirtualKey.Right);
            }
            else if (sentence.HasWordsInSequence(press, up)) {
                _windows.Keyboard.Press(VirtualKey.Up);
            }
            else if (sentence.HasWordsInSequence(press, down)) {
                _windows.Keyboard.Press(VirtualKey.Down);
            }
            return "";
        }

        public void OnOtherSubject() {
            
        }
    }
}