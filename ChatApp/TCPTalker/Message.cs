using System;

namespace TCPTalker
{
    public enum Header
    {
        NameRequest,
        TextMessage,
        ImageMessage,
        GIFMessage,
        MP3Message,
        NameAccepted,
        NameRejected,
    }

    [Serializable]
    public class Message
    {
        public Header MessageHeader;
        protected DateTime dateTime { get; set; }
        public string Time
        {
            get
            {
                return dateTime.ToShortTimeString();
            }
        }

        /// <summary>
        /// Sender's name (should only be filled by server)
        /// </summary>
        public string Sender { get; protected set; }

        public Message(Header header, DateTime time, string sender)
        {
            this.MessageHeader = header;
            this.dateTime = time;
            this.Sender = sender;
        }

        public override string ToString()
        {
            return "[" + Time + "] " + Sender;
        }

    }
}
