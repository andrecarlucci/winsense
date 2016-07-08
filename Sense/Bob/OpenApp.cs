using Dear;
using Sense.Profiles;

namespace Sense.Bob {
    public class OpenApp : ISubject {
        private readonly MrWindows _windows;

        public OpenApp(MrWindows windows) {
            _windows = windows;
        }

        public string GetAnswerFor(string sentence) {
            if (sentence.HasWordsInSequence("open","word|words")) {
                _windows.TaskManager.OpenApp(KnownApps.Word);
                return "Openning word";
            }
            if (sentence.HasWordsInSequence("open", "excel|xl")) {
                _windows.TaskManager.OpenApp(KnownApps.Excel);
                return "Openning excel";
            }
            if (sentence.HasWordsInSequence("open", "powerpoint")) {
                _windows.TaskManager.OpenApp(KnownApps.PowerPoint);
                return "Openning powerpoint";
            }
            return "";
        }

        public void OnOtherSubject() {
            
        }
    }
}