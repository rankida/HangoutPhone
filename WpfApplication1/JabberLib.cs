using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication1
{
    using System.Xml;

    using bedrock;

    using jabber;
    using jabber.client;
    using jabber.connection;
    using jabber.protocol.client;
    using jabber.protocol.iq;

    public class JabberLib
    {
        private string username;

        private string password;

        private string host;

        private int port = 5222;

        private JabberClient jabberClient;

        private CapsManager capabilityManager;

        public JabberLib(string username, string password, string host)
        {
            this.username = username;
            this.password = password;
            this.host = host;
        }

        /// <summary>
        /// Gets or sets the action to be performed when text is read
        /// in.
        /// </summary>
        public Action<string> OnReadText { get; set; }

        /// <summary>
        /// Gets or sets the action perfomred when things go wrong
        /// </summary>
        public Action<object, Exception> OnError { get; set; }

        /// <summary>
        /// Gets or sets the action called when the stream has a tizzy
        /// </summary>
        public Action<object, XmlElement> OnStreamError { get; set; }

        public void Connect(bool initialPresence)
        {
            this.jabberClient = new JabberClient();
            this.jabberClient.OnReadText += this.InternalOnReadText;
            this.jabberClient.OnError += new ExceptionHandler(this.OnError);
            this.jabberClient.OnStreamError += new jabber.protocol.ProtocolHandler(this.OnStreamError);

            // regisert presence
            this.jabberClient.AutoLogin = false;
            this.jabberClient.OnLoginRequired += this.jc_OnLoginRequired;
            this.jabberClient.OnRegisterInfo += this.jc_OnRegisterInfo;
            this.jabberClient.OnRegistered += this.jc_OnRegistered;

            var jid = new JID(this.username);
            this.jabberClient.User = jid.User;
            this.jabberClient.Server = jid.Server;
            this.jabberClient.NetworkHost = this.host;
            this.jabberClient.Port = this.port;
            this.jabberClient.Resource = "Hangout Phone";
            this.jabberClient.Password = this.password;
            this.jabberClient.AutoStartTLS = true;
            this.jabberClient.AutoPresence = initialPresence;
            this.jabberClient.AutoReconnect = 3f;

            this.capabilityManager = new CapsManager { Stream = this.jabberClient, Node = "http://rankida.com" };

            this.jabberClient.Connect();
        }

        private void InternalOnReadText(object sender, string txt)
        {
            if (string.IsNullOrEmpty(txt))
            {
                return;
            }

            Console.Out.WriteLine("RECV: " + txt);
            this.OnReadText(txt);
        }

        public void Write(string message)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.jabberClient.Close();
            this.jabberClient.Dispose();
            this.jabberClient = null;
        }

        private void jc_OnLoginRequired(object sender)
        {
            var jc = (JabberClient)sender;
            jc.Register(new JID(jc.User, jc.Server, null));
        }

        private void jc_OnRegistered(object sender, IQ iq)
        {
            var jc = (JabberClient)sender;
            if (iq.Type == IQType.result)
            {
                jc.Login();
            }
        }

        private bool jc_OnRegisterInfo(object sender, Register r)
        {
            return true;
        }
    }
}
