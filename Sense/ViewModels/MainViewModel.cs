using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using MrWindows;
using Sense.Profiles;
using Sense.Services;
using Sense.Util;
using SharpSenses;
using XamlActions;

namespace Sense.ViewModels {
    public class MainViewModel : ViewModelBase {
        private static object _sync = new object();
        private readonly ICamera _camera;
        private readonly Windows _windows;
        private readonly ProcessMonitor _processMonitor;

        private List<DateTime> _lastBlinks = new List<DateTime>();

        public ObservableCollection<Item> Items { get; set; }

        [Magic]
        public string CurrentProcess { get; set; }

        [Magic]
        public string CurrentProfile { get; set; }

        [Magic]
        public int TotalBlinks { get; set; }

        [Magic]
        public int BlinksPerMinute { get; set; }

        public MainViewModel(ICamera camera, Windows windows, ProcessMonitor processMonitor, ProfileManager profileManager) {
            _camera = camera;
            _windows = windows;
            _processMonitor = processMonitor;
            AddAllItems();
            BindBlinks();
            
            _processMonitor.ActiveProcessChanged += processName => {
                Dispatcher.Run(() => CurrentProcess = processName);
            };
            profileManager.ProfileChanged += profile => {
                Dispatcher.Run(() => CurrentProcess = profile.Name ?? "default");                
            };
        }

        private void BindBlinks() {
            _camera.Face.LeftEye.Blink += (sender, args) => {
                lock (_sync) {
                    TotalBlinks++;
                    _lastBlinks.Add(DateTime.Now);
                }
            };
            Task.Factory.StartNew(async () => {
                TimeSpan timeWindow = TimeSpan.FromSeconds(5);
                while (true) {
                    await Task.Delay(timeWindow);
                    lock (_sync) {
                        _lastBlinks.RemoveAll(x => x < DateTime.Now.Add(-TimeSpan.FromMinutes(1)));
                        BlinksPerMinute = _lastBlinks.Count(x => x < DateTime.Now.Add(-TimeSpan.FromSeconds(20))) *3;
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        private void AddAllItems() {
            Items = new ObservableCollection<Item>();
            AddItems(_camera.LeftHand);
            AddItems(_camera.LeftHand.GetAllFingers().SelectMany(f => f.GetAllJoints()).ToArray());
            AddItems(_camera.LeftHand.GetAllFingers().Cast<Item>().ToArray());
            AddItems(_camera.RightHand);
            AddItems(_camera.RightHand.GetAllFingers().SelectMany(f => f.GetAllJoints()).ToArray());
            AddItems(_camera.RightHand.GetAllFingers().Cast<Item>().ToArray());
        }
       
        private void AddItems(params Item[] items) {
            foreach (var item in items) {
                Items.Add(item);                
            }
        }

        public void Loaded() {
            StartCamera();
        }

        private void StartCamera() {
            //SetUpHand(_camera.RightHand);
            //SetUpHand(_camera.LeftHand);
            //SetUpFace(_camera);

            //SetupWindowsLogin();
            //ShowMessage("Camera Started");
        }

        
    }
}