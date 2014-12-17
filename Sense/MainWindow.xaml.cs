using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Hardcodet.Wpf.TaskbarNotification;
using MrWindows;
using Sense.Behaviors;
using Sense.Profiles;
using Sense.Util;
using SharpSenses;

namespace Sense {
    public partial class MainWindow : Window {
        private Windows _win;
        private ICamera _camera;
        private RealSenseCredentialPluginClient _client;

        private ProcessMonitor _processMonitor;
        private bool _faceMonitorActive;
        private DateTime _faceLastSeen;

        public int Width = 640;
        public int Height = 480;

        public MainWindow() {
            InitializeComponent();
            Left = SystemParameters.PrimaryScreenWidth - Width - 25;
            Top = SystemParameters.PrimaryScreenHeight - Height - 25;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e) {
            _win = App.Container.GetInstance<Windows>();
            StartProcessMonitor();
            Messenger.NewMesssage += s => {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    Message.Text = s
                ));
            };
            _camera = await FindCamera();
            StartCamera();
        }

        private void StartCamera() {
            ProfileManager.Init(_camera, _win, _processMonitor);
            ProfileManager.ProfileChanged += p => {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    Active.Text = p != null ? p.Name : "default"
                ));
            };
            SetUpHand(_camera.RightHand);
            SetUpHand(_camera.LeftHand);
            SetUpFace(_camera);

            SetupWindowsLogin();
            ShowMessage("Camera Started");
        }

        private void SetupWindowsLogin() {
            _client = new RealSenseCredentialPluginClient();
            _client.Start();

            var pLeft = new Point3D();
            var pRight = new Point3D();
            var action = new Action<string, Point3D, Point3D>((s, p1, p2) => {
                var x = SharpSenses.Util.MathEx.CalcDistance(p1, p2);
                //Console.WriteLine(s + "-> x1: {0} x2: {1} Dist-> {2}", p1.X, p2.X, x);
                if (x <= 80) {
                    _client.Authorize().Wait();
                }
            });
            _camera.LeftHand.Moved += (s, a) => {
                pLeft = a.NewPosition.Image;
                action.Invoke("L", pLeft, pRight);
            };
            _camera.RightHand.Moved += (s, a) => {
                pRight = a.NewPosition.Image;
                action.Invoke("R", pLeft, pRight);
            };
        }

        private void StartProcessMonitor() {
            _processMonitor = App.Container.GetInstance<ProcessMonitor>();
            _processMonitor.ActiveProcessChanged += processName => 
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                    ProcessName.Text = processName;
                })
            );
            _processMonitor.Start();
        }

        private async Task<ICamera> FindCamera() {
            while (true) {
                var camera = App.Container.GetInstance<ICamera>();
                try {
                    camera.Start();
                    ShowMessage("RealSense Camera Started");
                    return camera;
                }
                catch (CameraException) {
                    ShowMessage("Please, connect the RealSense Camera");
                }
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        private void SetUpFace(ICamera camera) {
            Task.Run(async () => {
                while (true) {
                    await Task.Delay(1000);
                    if (!_faceMonitorActive) {
                        continue;
                    }
                    if (!camera.Face.IsVisible) {
                        Debug.WriteLine("Locking: " + (DateTime.Now - _faceLastSeen).TotalSeconds);
                    }
                    if (!camera.Face.IsVisible && (DateTime.Now - _faceLastSeen) > TimeSpan.FromSeconds(5)) {
                        _win.LockWorkStation();
                    }
                }
            });
            camera.Face.Moved += (s, a) => {
                DisplayBodyPart(camera.Face, 30);
            };
            camera.Face.Visible += (s, a) => {
                _faceLastSeen = DateTime.Now;
                Debug.WriteLine("Face visible");
            };
            camera.Face.NotVisible += (s, a) => {
                _faceLastSeen = DateTime.Now;
                Debug.WriteLine("Face not visible");
            };
        }

        private void SetUpHand(Hand hand) {
            SetUpItem(20, hand);
            SetUpItem(hand.Thumb, hand.Index, hand.Middle, hand.Ring, hand.Pinky);
            _camera.RightHand.NotVisible += (s, a) => _win.Mouse.MouseLeftUp();
            _camera.LeftHand.NotVisible += (s, a) => _win.Mouse.MouseLeftUp();
        }

        private void SetUpItem(params Item[] items) {
            SetUpItem(5, items);
        }

        private void SetUpItem(int size, params Item[] items) {
            for (int i = 0; i < items.Length; i++) {
                int i1 = i;
                items[i].Moved += (s, a) => DisplayBodyPart(items[i1], size);
                items[i].NotVisible += (s, a) => DisplayBodyPart(items[i1], size);                
            }
        }

        private void ShowMessage(string title, string message = " ", BalloonIcon icon = BalloonIcon.Info) {
            NotifyIcon.ShowBalloonTip(title, message, icon);
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            NotifyIcon.Dispose();
        }

        private Dictionary<Item, Ellipse> _parts = new Dictionary<Item, Ellipse>();
        
        private void DisplayBodyPart(Item part, int size = 5) {
            Action action = delegate {
                EnsureElipse(part, size);
                var ellipse = _parts[part];
                Color color = part.IsVisible ? Color.FromArgb(100, 100, 200, 100) : Colors.Transparent;
                ellipse.Fill = new SolidColorBrush(color);

                var p = CameraToScreenMapper.MapToScreen(part.Position.Image, Width, Height);
                if (p.X < 0) {
                    p.X = 0;
                }
                Canvas.SetLeft(ellipse, p.X);
                Canvas.SetTop(ellipse, p.Y);
            };
            Dispatcher.Invoke(action);
        }

        private void EnsureElipse(Item part, int size) {
            if (_parts.Keys.Contains(part)) return;
            var shape = new Ellipse {
                Width = size,
                Height = size
            };
            _parts.Add(part, shape);
            HandCanvas.Children.Add(shape);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton != MouseButton.Left) {
                return;
            }
            DragMove();
        }

        private void MenuItem_OnChecked(object sender, RoutedEventArgs e) {
            _faceMonitorActive = LockScreenMonitor.IsChecked;
        }

        private void PanelVisible_OnChecked(object sender, RoutedEventArgs e) {
            Visibility = PanelVisible.IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}