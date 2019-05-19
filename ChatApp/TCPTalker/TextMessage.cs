using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPTalker
{
    [Serializable]
    public class TextMessage :  Message
    {
        public string Text { get; set; }

        public TextMessage(DateTime dateTime , string sender, string text) : base(Header.TextMessage,dateTime,sender)
        {
            this.Text = text;
        }

        public override string ToString()
        {
            return base.ToString() + " : " + Text;
        }
    }
}
