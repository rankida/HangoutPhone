namespace WpfApplication1
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Xml;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private JabberLib jabber;

        public Window1()
        {
            InitializeComponent();
        }

        private void connect_Click(object sender, RoutedEventArgs e)
        {
            var username = this.usernameBox.Text;
            var password = this.passwordBox.Password;
            var host = "talk.google.com";

            this.jabber = new JabberLib(username, password, host)
                {
                    OnReadText = this.Display,
                    OnWriteText = this.DisplaySent,
                    OnError = this.OnError,
                    OnStreamError = this.OnStreamError
                };
            this.jabber.Connect(true);
            this.messageBox.IsEnabled = true;
            this.send.IsEnabled = true;
            this.connect.IsEnabled = false;
            this.close.IsEnabled = true;
        }

        private void Display(string raw)
        {
            this.chatDisplay.BeginActionInvoke(() => this.chatDisplay.AppendText("RECV: " + raw + Environment.NewLine));
        }

        private void DisplaySent(string raw)
        {
            this.chatDisplay.BeginActionInvoke(() => this.chatDisplay.AppendText("SENT: " + raw + Environment.NewLine));
        }

        private void OnError(object arg1, Exception ex)
        {
            this.chatDisplay.BeginActionInvoke(() => this.chatDisplay.AppendText(@"{\rtf1\ansi Error: \b" + ex.Message + @"\b0 }" + Environment.NewLine));
        }

        private void OnStreamError(object arg1, XmlElement arg2)
        {
            this.chatDisplay.BeginActionInvoke(() => this.chatDisplay.AppendText(@"{\rtf1\ansi StreamError: \b" + arg2 + @"\b0 }" + Environment.NewLine));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.jabber != null)
            {
                this.jabber.Close();
            }

            base.OnClosing(e);
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            if (this.jabber != null)
            {
                this.jabber.Close();
            }

            this.close.IsEnabled = false;
            this.connect.IsEnabled = true;
        }
    }
}
