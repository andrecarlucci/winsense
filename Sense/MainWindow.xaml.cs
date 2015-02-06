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
using Sense.Lockscreen;
using Sense.Profiles;
using Sense.Services;
using Sense.Storage;
using Sense.Util;
using SharpSenses;
using XamlActions;

namespace Sense {
    public partial class MainWindow : Window {
        private Windows _win;
        private ICamera _camera;
        private RealSenseCredentialPluginClient _client;

        private ProcessMonitor _processMonitor;
        private bool _faceMonitorActive;
        private DateTime _faceLastSeen;

        public static int MyWidth = 320;
        public static int MyHeight = 240;

        public static TaskbarIcon TaskbarIcon;

        public MainWindow() {
            InitializeComponent();
            Left = SystemParameters.PrimaryScreenWidth - MyWidth - 150;
            Top = SystemParameters.PrimaryScreenHeight - MyHeight - 40;
            Item.DefaultNoiseThreshold = 2;
            TaskbarIcon = NotifyIcon;
            Mediator.Default.Subscribe<NotifyIconMessage>(this, ShowNotifyIconMessage);
        }

        private void ShowNotifyIconMessage(NotifyIconMessage message) {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                NotifyIcon.ShowBalloonTip(message.Title, message.Text, message.BalloonIcon)
            ));
        }

        private void StartCamera() {
            //ProfileManager.Init(_camera, _win, _processMonitor);
            //ProfileManager.ProfileChanged += p => {
            //    Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            //        Active.Text = p != null ? p.Name : "default"
            //    ));
            //};
            //SetUpHand(_camera.RightHand);
            //SetUpHand(_camera.LeftHand);
            //SetUpFace(_camera);

            //SetupWindowsLogin();
            //ShowMessage("Camera Started");
        }

        private void SetupWindowsLogin() {
            var unlocker = new Unlocker(new RealSenseCredentialPluginClient(), _camera);
            unlocker.Start();
        }
       
        private void SetUpFace(ICamera camera) {
            _camera.Face.FaceRecognized += FaceOnFaceRecognized; 

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
            SetUpItem(hand.Thumb, hand.Thumb.BaseJoint, hand.Thumb.FirstJoint, hand.Thumb.SecondJoint);
            SetUpItem(hand.Index, hand.Index.BaseJoint, hand.Index.FirstJoint, hand.Index.SecondJoint);
            SetUpItem(hand.Middle, hand.Middle.BaseJoint, hand.Middle.FirstJoint, hand.Middle.SecondJoint);
            SetUpItem(hand.Ring, hand.Ring.BaseJoint, hand.Ring.FirstJoint, hand.Ring.SecondJoint);
            SetUpItem(hand.Pinky, hand.Pinky.BaseJoint, hand.Pinky.FirstJoint, hand.Pinky.SecondJoint);
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
            NotifyIcon.Icon = null;
            NotifyIcon.Dispose();
        }

        private Dictionary<Item, Ellipse> _parts = new Dictionary<Item, Ellipse>();
        
        private void DisplayBodyPart(Item part, int size = 5) {
            Action action = delegate {
                EnsureElipse(part, size);
                var ellipse = _parts[part];
                Color color = part.IsVisible ? Color.FromArgb(100, 100, 200, 100) : Colors.Transparent;
                ellipse.Fill = new SolidColorBrush(color);

                var p = CameraToScreenMapper.MapToScreen(part.Position.Image, MyWidth, MyHeight);
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
            //HandCanvas.Children.Add(shape);
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

        private void Exit_OnClick(object sender, RoutedEventArgs e) {
            Close();
        }

        private void RegisterCurrentUser_OnClick(object sender, RoutedEventArgs e) {
            _camera.Face.RecognizeFace();
        }

        private int _lastFaceId;

        private void FaceOnFaceRecognized(object sender, FaceRecognizedEventArgs args) {
            if (args.UserId == _lastFaceId) {
                return;
            }
            _lastFaceId = args.UserId;
            Config.Default.Set(ConfigKeys.UserId, args.UserId);
        }
    }
}