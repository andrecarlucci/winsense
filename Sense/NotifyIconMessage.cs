using Hardcodet.Wpf.TaskbarNotification;

namespace Sense {
    public class NotifyIconMessage {
        public string Title { get; set; }
        public string Text { get; set; }
        public BalloonIcon BalloonIcon  { get; set; }

        public NotifyIconMessage(string title, string text = " ", BalloonIcon icon = BalloonIcon.Info) {
            Title = title;
            Text = text;
            BalloonIcon = icon;
        }
    }
}