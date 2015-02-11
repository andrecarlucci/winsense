using System;
using System.Threading;
using System.Windows.Threading;
using Sense.Controls;

namespace Sense.Util {
    public class InputService : IInputService {
        private ManualResetEvent _mre = new ManualResetEvent(false);
        private string _result;
        public string GetInput(string title, string message) {
            _result = null;
            App.Current.Dispatcher.BeginInvoke(new Action(GetUsername));
            _mre.WaitOne();
            return _result;
        }

        public void GetUsername() {
            var dialog = new InputDialog("Please, enter your name", "Face recognized!");
            dialog.Topmost = true;
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value) {
                _result = dialog.Answer;
            }
            else {
                _result = null;                     
            }
            _mre.Set();
        }
    }
}