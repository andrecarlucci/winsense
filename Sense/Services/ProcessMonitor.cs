using System;
using System.Threading.Tasks;
using Dear;

namespace Sense.Services {
    public class ProcessMonitor {
        private MrWindows _windows;
        public string CurrentProcess { get; private set; }
        public event Action<string> ActiveProcessChanged;
        public event Action<string> ActiveProcessLoop;

        public ProcessMonitor(MrWindows windows) {
            _windows = windows;
        }

        public void Start() {
            Task.Run(async () => {
                while (true) {
                    await Task.Delay(500);
                    var process = _windows.CurrentWindow.GetProcessName();
                    if (process != CurrentProcess) {
                        OnActiveProcessChanged(process);
                        CurrentProcess = process;
                    }
                    OnActiveProcessLoop(process);
                }
            });
        }

        protected virtual void OnActiveProcessLoop(string name) {
            var handler = ActiveProcessLoop;
            if (handler != null) handler(name);
        }

        protected virtual void OnActiveProcessChanged(string name) {
            var handler = ActiveProcessChanged;
            if (handler != null) handler(name);
        }
    }
}