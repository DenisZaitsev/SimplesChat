using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TCPTalker;

namespace ChatServer
{
    static class Program
    {
        static Dictionary<TcpClient, string> users = new Dictionary<TcpClient, string>();
        static byte[] buffer = new byte[256];

        static void Main(string[] args)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);
            TcpListener tcpListener = new TcpListener(ip);
            IPHostEntry hostname = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] localIP = hostname.AddressList;
            List<Thread> threads = new List<Thread>();
            foreach (var IP in localIP)
                Console.WriteLine(IP.ToString());
            tcpListener.Start(5);
            Console.WriteLine("Server started");

            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                Console.WriteLine("Connection made");

                Thread t = new Thread(() => TalkToClient(client));
                threads.Add(t);
                t.Start();

            }
        }


       /// <summary>
       /// Receive message in loop and prepare answer
       /// </summary>
       /// <param name="client"></param>
        static private void TalkToClient(TcpClient client)
        {
            while (true)
            {
                //if client has disconnected, remove him from the list
                if (!client.Connected)
                {
                    if (users.Keys.Contains(client))
                    {                     
                        Broadcast(new Message(Header.TextMessage, DateTime.Now, "Server", String.Format("User {0} disconnected", users[client])));
                        users.Remove(client);
                        Log(String.Format("User {0} disconnected", users[client]));
                    }
                    else
                    {
                        Log("Couldn't register user");
                    }
                    break;
                }
                Message receivedMessage = TCPHelper.ReceiveMessage(client);

                switch (receivedMessage.MessageHeader)
                {

                    case Header.NameRequest:
                        //In case of name request, check is such name is taken, if so, reject this name
                        if (users.Values.Contains(receivedMessage.Text))
                        {
                            TCPHelper.SendMessage(new Message(Header.NameRejected, DateTime.Now, "Server", "Name rejected"), client);
                            Log(String.Format("Username {0} taken", receivedMessage.Text));
                            client.Close();
                        }
                        else
                        {
                            //If we already have a connected user with a name, change his name to a new one and send accept message. Also, tell every user about the name change
                            if (users.Keys.Contains(client))
                            {
                                string previousName = users[client];
                                users[client] = receivedMessage.Text;
                                TCPHelper.SendMessage(new Message(Header.NameAccepted, DateTime.Now, "Server", "Name changed"), client);
                                Broadcast(new Message(Header.TextMessage, DateTime.Now, "Server", String.Format("User {0} is now {1}", previousName, users[client])));
                                Log(String.Format("User {0} is now {1}", previousName, users[client]));
                            }
                            //If this is a new user, add him to our user list, send him accept message, and also notify everyone about new user
                            else
                            {
                                users.Add(client, receivedMessage.Text);
                                TCPHelper.SendMessage(new Message(Header.NameAccepted, DateTime.Now, "Server", "Name accepted"), client);
                                Broadcast(new Message(Header.TextMessage, DateTime.Now, "Server", String.Format("User {0} has joined the chat", receivedMessage.Text)));
                                Log(String.Format("User {0} has joined the chat", users[client]));
                            }
                        }
                        break;

                    //If we get a textmessage, send it to everyone
                    case Header.TextMessage:
                        if (users.Keys.Contains(client))
                        {
                            Broadcast(new Message(Header.TextMessage, DateTime.Now, users[client], receivedMessage.Text));
                            Log(new Message(receivedMessage.MessageHeader, DateTime.Now, users[client], receivedMessage.Text).ToString());
                        }
                        else
                        {
                            Log("Uknown message: " + receivedMessage.ToString());
                            client.Close();
                        }
                        break;

                    default:
                        Log("Uknown message: " + receivedMessage.ToString());
                        client.Close();
                        break;
                }
            }
        }


        //send message to every user
        static private void Broadcast(Message message)
        {
            foreach (TcpClient client in users.Keys)
                TCPHelper.SendMessage(message, client);
        }


        //Write info to local file and console, if file is taken, wait just a tiny bit
        static private void Log(string entry)
        {
            while (true)
                try
                {
                    using (StreamWriter sw = new StreamWriter(File.OpenWrite("log.txt")))
                    {
                        sw.WriteLine(DateTime.Now.ToString() + " " + entry);
                    }
                    break;
                }
                catch
                {
                    Thread.Sleep(5);
                }
            Console.WriteLine(entry);
        }

    }
}
