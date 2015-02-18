using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Hardcodet.Wpf.TaskbarNotification;
using Sense.Services;
using XamlActions;

namespace Sense {
    public partial class MainWindow : Window {
        public static int MyWidth = 320;
        public static int MyHeight = 240;
        public static TaskbarIcon TaskbarIcon;

        public MainWindow() {
            InitializeComponent();
            Left = SystemParameters.PrimaryScreenWidth - MyWidth - 150;
            Top = SystemParameters.PrimaryScreenHeight - MyHeight - 40;
            TaskbarIcon = NotifyIcon;
            Mediator.Default.Subscribe<NotifyIconMessage>(this, ShowNotifyIconMessage);
        }

        private void ShowNotifyIconMessage(NotifyIconMessage message) {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                NotifyIcon.ShowBalloonTip(message.Title, message.Text, message.BalloonIcon)
                ));
        }

        private void ShowMessage(string title, string message = " ", BalloonIcon icon = BalloonIcon.Info) {
            NotifyIcon.ShowBalloonTip(title, message, icon);
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            NotifyIcon.Icon = null;
            NotifyIcon.Dispose();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton != MouseButton.Left) {
                return;
            }
            DragMove();
        }

        private void LockscreenWatcher_OnChecked(object sender, RoutedEventArgs e) {
            Mediator.Default.Publish(new LockscreenEnabledMessage(LockScreenMonitorMenu.IsChecked));
            ChangeWatcherText();
        }

        private void Watcher_OnClick(object sender, RoutedEventArgs e) {
            LockScreenMonitorMenu.IsChecked = !LockScreenMonitorMenu.IsChecked;
            ChangeWatcherText();
        }

        private void ChangeWatcherText() {
            WatcherText.Text = LockScreenMonitorMenu.IsChecked ? "on" : "off";        
        }

        private void PanelVisible_OnChecked(object sender, RoutedEventArgs e) {
            ChangeVisibility();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e) {
            Close();
        }

        private void RegisterCurrentUser_OnClick(object sender, RoutedEventArgs e) {}

        private void ItemsControl_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            PanelVisible.IsChecked = false;
            ChangeVisibility();
        }

        private void ChangeVisibility() {
            Visibility = PanelVisible.IsChecked ? Visibility.Visible : Visibility.Collapsed;        
        }

        private void NotifyIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e) {
            PanelVisible.IsChecked = !PanelVisible.IsChecked;
            ChangeVisibility();
        }

    }
}