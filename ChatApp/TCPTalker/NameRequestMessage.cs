using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPTalker
{
    [Serializable]
    public class NameRequestMessage : Message
    {
        public string NameToRequest;
        public NameRequestMessage(DateTime dateTime,string sender, string name):base(Header.NameRequest,dateTime,sender)
        {
            this.MessageHeader = Header.NameRequest;
            this.NameToRequest = name;
        }
    }
}
