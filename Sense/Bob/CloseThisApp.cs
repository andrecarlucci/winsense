using System.CodeDom;
using Dear;

namespace Sense.Bob {
    public class CloseThisApp : ISubject {
        private readonly MrWindows _windows;

        private bool _stepConfirm;

        public CloseThisApp(MrWindows windows) {
            _windows = windows;
        }

        public string GetAnswerFor(string sentence) {
            const string close = "close|coasters|blows|glows|grows|cruise|those|both|crudo|rose|lowes|those";
            const string pthis = "this|at|just|dates|day|dicks|dick|case|face|do's|duties|deuce|jesus";
            if (!_stepConfirm && sentence.HasWordsInSequence(close, pthis)) {
                _stepConfirm = true;
                var process = _windows.CurrentWindow.GetProcessName();
                return "I'm about to close " + process + ". Are you sure?";
            }
            if (_stepConfirm) {
                _stepConfirm = false;
                if (sentence.Contains("yes")) {
                    _windows.CurrentWindow.GetForegroundProcess().Kill();
                    return "Done.";
                }
                return "No problem.";
            }
            return "";
        }

        public void OnOtherSubject() {
            _stepConfirm = false;
        }
    }
}