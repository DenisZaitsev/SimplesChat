using System;

namespace TCPTalker
{
    public enum Header
    {
        NameRequest,
        TextMessage,
        NameAccepted,
        NameRejected,
    }

    [Serializable]
    public class Message
    {
        public Header MessageHeader;
        private DateTime dateTime { get; set; }
        public string Time
        {
            get
            {
                return dateTime.ToShortTimeString();
            }
        }
        public string Sender { get; private set; }
        public string Text { get; private set; }

        public Message(Header header, DateTime time, string sender, string text)
        {
            this.MessageHeader = header;
            this.dateTime = time;
            this.Sender = sender;
            this.Text = text;
        }

        public override string ToString()
        {
            return "[" + Time + "] " + Sender + ": " + Text;
        }

    }
}
