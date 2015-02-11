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
using Sense.Storage;
using Sense.Util;
using SharpSenses;
using XamlActions;

namespace Sense.ViewModels {
    public class MainViewModel : ViewModelBase {
        private static readonly object _sync = new object();
        private readonly ICamera _camera;
        private readonly Windows _windows;
        private readonly ProcessMonitor _processMonitor;
        private readonly UserWatcher _userWatcher;

        private readonly List<DateTime> _lastBlinks = new List<DateTime>();
        private string _username;

        public ObservableCollection<Item> Items { get; set; }
        [Magic]
        public string CurrentProcess { get; set; }
        [Magic]
        public string CurrentProfile { get; set; }
        [Magic]
        public int TotalBlinks { get; set; }
        [Magic]
        public int BlinksPerMinute { get; set; }
        [Magic]
        public int Smiles { get; set; }
        [Magic]
        public string Username { get; set; }
        public MainViewModel(ICamera camera, 
                             Windows windows, 
                             ProcessMonitor processMonitor, 
                             ProfileManager profileManager,
                             UserWatcher userWatcher) {
            _camera = camera;
            _windows = windows;
            _processMonitor = processMonitor;
            _userWatcher = userWatcher;
            AddAllItems();
            BindBlinks();
            BindSmiles();
            
            Mediator.Default.Subscribe<UserChangedMessage>(this, message => Username = message.Value);
            
            _processMonitor.ActiveProcessChanged += processName => {
                Dispatcher.Run(() => CurrentProcess = processName);
            };
            profileManager.ProfileChanged += profile => {
                var name = profile == null ? "default" : profile.Name;
                Dispatcher.Run(() => CurrentProfile = name);
            };
        }

        public void RecognizeUser() {
            _userWatcher.RecognizeUser();
        }

        private void BindSmiles() {
            _camera.Face.Mouth.Smiled += (sender, args) => {
                Smiles++;
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
            AddItems(_camera.Face);
            Task.Run(async () => {
                while (true) {
                    await Task.Delay(2000);
                    foreach (var item in Items) {
                        item.IsVisible = false;
                    }                    
                }
            });
        }
       
        private void AddItems(params Item[] items) {
            foreach (var item in items) {
                Items.Add(item);                
            }
        }
    }
}