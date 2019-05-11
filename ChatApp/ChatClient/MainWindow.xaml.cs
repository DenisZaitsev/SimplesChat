using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Threading;
using System.Xaml;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TCPTalker;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Connection serverConnection;
        Session session;

        public MainWindow(Connection connection)
        {
            InitializeComponent();
            this.serverConnection = connection;
                              
        }

        //Recieve message and add it to the list of messages
        public void getMessage()
        {
            while (true)
            {
                if(!serverConnection.tcpClient.Connected)
                {
                    MessageBox.Show("Connection closed!");
                    Application.Current.Dispatcher.Invoke(new Action(() => { this.Close(); }));
                    break;
                }
                Message receivedMessage = TCPHelper.ReceiveMessage(serverConnection.tcpClient);

                switch (receivedMessage.MessageHeader)
                {
                    case Header.TextMessage:
                        try
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() => { session.LocalMessages.Add(receivedMessage); }));
                        }
                        catch { Console.WriteLine("UI Invoke exception caught"); }
                        break;                 
                }
            }

        }

        public void sendButtonClick(object sender, EventArgs e)
        {
            TCPHelper.SendTextMessage(session.MessageToSend, serverConnection.tcpClient);           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            session = FindResource("session") as Session;
            Thread thread = new Thread(new ThreadStart(getMessage));
            thread.Start();          
        }
    }
      
}
