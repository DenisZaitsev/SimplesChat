using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace TCPTalker
{
    public static class TCPHelper
    {       
        /// <summary>
        /// sends a simple message where only header matters (right now used for registering a new name)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="tcpClient"></param>
        public static void SendMessage(Message message, TcpClient tcpClient)
        {
            BinaryFormatter bf = new BinaryFormatter();

            //Send serialized message
            using (MemoryStream memStream = new MemoryStream())
            {
                bf.Serialize(memStream, message);
                tcpClient.GetStream().Write(memStream.ToArray(), 0, Convert.ToInt32(memStream.Length));
            }
        }


        public static bool Autenficate(string name, TcpClient tcpClient)
        {
            //Send name request
            SendMessage(new NameRequestMessage(DateTime.Now, name, name),tcpClient);
            //Receive server's answer
            Message receivedMessage = ReceiveMessage(tcpClient);
            //Check message for right header, server sends "Accepted" in messages Text property if everything's fine
            return (receivedMessage.MessageHeader == Header.NameAccepted);

        }

        /// <summary>
        /// Send a simple string text message 
        /// </summary>
        /// <param name="message"></param>
        public static void SendTextMessage(string message,string name,TcpClient tcpClient)
        {
            BinaryFormatter bf = new BinaryFormatter();

            using (MemoryStream memStream = new MemoryStream())
            {
                bf.Serialize(memStream, new TextMessage(DateTime.Now, null, message));
                tcpClient.GetStream().Write(memStream.ToArray(), 0, Convert.ToInt32(memStream.Length));
            }
        }

        /// <summary>
        /// send an image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="name"></param>
        /// <param name="tcpClient"></param>
        public static void SendImageMessage(Bitmap image,string name, TcpClient tcpClient)
        {
            BinaryFormatter bf = new BinaryFormatter();        

            using (MemoryStream memStream = new MemoryStream())
            {
                bf.Serialize(memStream, new ImageMessage(DateTime.Now, name, image));
                tcpClient.GetStream().Write(memStream.ToArray(), 0, Convert.ToInt32(memStream.Length));
            }
        }

        /// <summary>
        /// Wait for message syncronyosly
        /// </summary>
        /// <returns></returns>
        public static Message ReceiveMessage(TcpClient tcpClient)
        {
            BinaryFormatter bf = new BinaryFormatter();
            byte[] buffer = new byte[256];

            NetworkStream netStream = tcpClient.GetStream();

            //Get answer message
            using (MemoryStream memStream = new MemoryStream())
            {
                try
                {
                    do
                    {
                        int bytesread = netStream.Read(buffer, 0, buffer.Length);
                        memStream.Write(buffer, 0, bytesread);
                    }
                    while (netStream.DataAvailable);
                }
                catch
                {
                    tcpClient.Close();                  
                }
                             
                memStream.Seek(0, SeekOrigin.Begin);
                try
                {
                    return (Message)bf.Deserialize(memStream);
                }
                catch
                {
                    return new TextMessage(DateTime.Now, "INTERNAL ERROR", "GOT BAD MESSAGE");
                }
                
            }
        }
    }
}
