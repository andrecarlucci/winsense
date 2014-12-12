﻿using System;
using System.Threading.Tasks;
using MrWindows;

namespace Sense {
    public class ProcessMonitor {
        private Windows _windows;
        public string CurrentProcess { get; private set; }
        public event Action<string> ActiveProcessChanged;
        public event Action<string> ActiveProcessLoop;

        public ProcessMonitor(Windows windows) {
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