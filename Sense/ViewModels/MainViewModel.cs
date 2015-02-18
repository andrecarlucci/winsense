using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;
using MrWindows;
using Sense.Profiles;
using Sense.Services;
using Sense.Storage;
using Sense.Util;
using SharpSenses;
using SharpSenses.Gestures;
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
        public int Yawms { get; set; }

        [Magic]
        public string Username { get; set; }

        [Magic]
        public SolidColorBrush EyeUpColor { get; set; }
        [Magic]
        public SolidColorBrush EyeDownColor { get; set; }
        [Magic]
        public SolidColorBrush EyeLeftColor { get; set; }
        [Magic]
        public SolidColorBrush EyeRightColor { get; set; }

        private static SolidColorBrush _blue = new SolidColorBrush(Colors.Blue);
        private static SolidColorBrush _red = new SolidColorBrush(Colors.Red);

        public MainViewModel(ICamera camera, 
                             Windows windows, 
                             ProcessMonitor processMonitor, 
                             ProfileManager profileManager,
                             UserWatcher userWatcher) {
            _camera = camera;
            _windows = windows;
            _processMonitor = processMonitor;
            _userWatcher = userWatcher;
            Items = new ObservableCollection<Item>();
            AddAllItems();
            BindBlinks();
            BindSmiles();
            BindYawns();

            EyeUpColor = _blue;
            EyeDownColor = _blue;
            EyeLeftColor = _blue;
            EyeRightColor = _blue;

            var _eyes = new Dictionary<Direction, Action<SolidColorBrush>> {
                {Direction.Up, c => EyeUpColor = c},
                {Direction.Down, c => EyeDownColor= c},
                {Direction.Left, c => EyeLeftColor= c},
                {Direction.Right, c => EyeRightColor= c},
                {Direction.None, c => { }}
            };

            camera.Face.EyesDirectionChanged += (sender, args) => {
                _eyes[args.NewDirection].Invoke(_red);
                Task.Run(async () => {
                    await Task.Delay(1000);
                    _eyes[args.NewDirection].Invoke(_blue);
                });
            };

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
            Username = "wait...";
            _userWatcher.RecognizeUser();
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

        private void BindSmiles() {
            _camera.Face.Mouth.Smiled += (sender, args) => {
                Smiles++;
            };
        }

        private void BindYawns() {
            _camera.Face.Yawned += (sender, args) => {
                Yawms++;
            };
        }

        //private void AddAllItemsLoop() {
        //    Items = new ObservableCollection<Item>();
        //    Task.Run(async () => {
        //        while (true) {
        //            await Task.Delay(2000);
        //            //Items.Clear();
        //            AddAllItems();
        //        }
        //    });
        //}

        public void AddAllItems() {
            AddItems(_camera.LeftHand);
            AddItems(_camera.LeftHand.GetAllFingers().SelectMany(f => f.GetAllJoints()).ToArray());
            AddItems(_camera.LeftHand.GetAllFingers().Cast<Item>().ToArray());
            AddItems(_camera.RightHand);
            AddItems(_camera.RightHand.GetAllFingers().SelectMany(f => f.GetAllJoints()).ToArray());
            AddItems(_camera.RightHand.GetAllFingers().Cast<Item>().ToArray());
            AddItems(_camera.Face);
            AddItems(_camera.Face.LeftEye);
            AddItems(_camera.Face.RightEye);
            AddItems(_camera.Face.Mouth);
            
        }
       
        private void AddItems(params Item[] items) {
            foreach (var item in items) {
                Items.Add(item);                
            }
        }
    }
}