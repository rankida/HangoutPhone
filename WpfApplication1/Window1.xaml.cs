using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    using System.ComponentModel;
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
                    OnReadText = this.Display, OnError = this.OnError, OnStreamError = this.OnStreamError 
                };
            this.jabber.Connect(true);
            this.messageBox.IsEnabled = true;
            this.send.IsEnabled = true;
        }

        private void Display(string raw)
        {
            this.chatDisplay.Dispatcher.BeginInvoke().AppendText(raw);
        }

        private void OnError(object arg1, Exception ex)
        {
            throw new NotImplementedException("Error: " + ex.Message);
        }

        private void OnStreamError(object arg1, XmlElement arg2)
        {
            throw new NotImplementedException("Stream Error");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.jabber.Close();
            base.OnClosing(e);
        }
    }
}
