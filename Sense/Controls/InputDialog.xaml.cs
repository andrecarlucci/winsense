using System;
using System.Windows;

namespace Sense.Controls {
    /// <summary>
    ///     Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window {
        public InputDialog(string question, string title = "WinSenses") {
            InitializeComponent();
            QuestionLabel.Content = question;
            Title = title;
        }

        private void OkPressed(object sender, RoutedEventArgs e) {
            DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e) {
            AnswerTextBox.SelectAll();
            AnswerTextBox.Focus();
        }

        public string Answer {
            get {
                return AnswerTextBox.Text;
            }
        }
    }
}