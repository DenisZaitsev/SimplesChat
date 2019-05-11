using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TCPTalker;

namespace ChatClient
{

    public class Connection
    {
        public TcpClient tcpClient;

        public Connection(IPEndPoint server)
        { 
            tcpClient = new TcpClient();
            tcpClient.Connect(server);
        }


        /// <summary>
        /// Asks server if the name is taken
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Returns true if name is OK and false if name is wrong</returns>
        public bool Autenficate(string name)
        {
            return TCPHelper.Autenficate(name, tcpClient);       
        }

        /// <summary>
        /// Send a simple string text message 
        /// </summary>
        /// <param name="message"></param>
        public void SendTextMessage(string message)
        {
            TCPHelper.SendTextMessage(message, tcpClient);
        }

        /// <summary>
        /// Send any kind of message (helper)
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(Message message)
        {
            TCPHelper.SendMessage(message, tcpClient);
        }

        /// <summary>
        /// Wait for message syncronyosly
        /// </summary>
        /// <returns></returns>
        public Message ReceiveMessage()
        {
            return TCPHelper.ReceiveMessage(tcpClient);
        }
    }
}
