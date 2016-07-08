using Dear;
using Sense.Storage;

namespace Sense.Bob {
    public class LockTheScreen : ISubject {
        private readonly MrWindows _windows;

        public LockTheScreen(MrWindows windows) {
            _windows = windows;
        }
        public string GetAnswerFor(string sentence) {
            if (sentence.HasWordsInSequence("sarah", "lock|look|what|luck", "screen")) {
                _windows.LockWorkStation();
                return "Screen locked as requested";
            }
            return "";
        }

        public void OnOtherSubject() {
        }
    }
}