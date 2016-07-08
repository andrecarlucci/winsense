using Dear;

namespace Sense.Bob {
    public class GoogleThis : ISubject {

        private readonly MrWindows _windows;

        public GoogleThis(MrWindows windows) {
            _windows = windows;
        }

        public string GetAnswerFor(string sentence) {
            if (sentence.Trim().StartsWith("sarah")) {
                sentence = sentence.Replace("sarah", "");
            }
            if (sentence.StartsWithOneOf("google|go-go")) {
                var search = sentence.GetAllAfter("google");
                _windows.TaskManager.GoogleThis(search);
                return "Let me google that for you";
            }
            return "";
        }

        public void OnOtherSubject() {
        }
    }
}