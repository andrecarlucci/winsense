using MrWindows;
using Sense.Profiles;

namespace Sense.Bob {
    public class OpenApp : ISubject {
        private readonly Windows _windows;

        public OpenApp(Windows windows) {
            _windows = windows;
        }

        public string GetAnswerFor(string sentence) {
            if (sentence.HasWordsInSequence("open","word|words")) {
                _windows.OpenApp(KnownApps.Word);
                return "Openning word";
            }
            if (sentence.HasWordsInSequence("open", "excel|xl")) {
                _windows.OpenApp(KnownApps.Excel);
                return "Openning excel";
            }
            if (sentence.HasWordsInSequence("open", "powerpoint")) {
                _windows.OpenApp(KnownApps.PowerPoint);
                return "Openning powerpoint";
            }
            return "";
        }

        public void OnOtherSubject() {
            
        }
    }
}