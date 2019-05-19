using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Threading;
using TCPTalker;
using Microsoft.Win32;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Connection serverConnection;
        Session session;


        //TODO bind image in XAML
        public MainWindow(Connection connection)
        {
            InitializeComponent();
            this.serverConnection = connection;

            pictureImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("../../IconResources/imageIcon.png")));
        }

        //Recieve message and add it to the list of messages
        public void getMessage()
        {
            while (true)
            {
                //checking for remaining connection
                if (!serverConnection.tcpClient.Connected)
                {
                    MessageBox.Show("Connection closed!");
                    if(Application.Current.Dispatcher!=null)
                    Application.Current.Dispatcher.Invoke(new Action(() => { this.Close(); }));
                    break;
                }
                Message receivedMessage = TCPHelper.ReceiveMessage(serverConnection.tcpClient);

                //receive message and handle it by adding to chat list
                switch (receivedMessage.MessageHeader)
                {   
                    case Header.TextMessage:
                        try
                        {
                            TextMessage textMessage = receivedMessage as TextMessage;
                            Application.Current.Dispatcher.Invoke(new Action(() => { session.LocalMessages.Add(textMessage); }));
                        }
                        catch { Console.WriteLine("UI Invoke exception caught"); }
                        break;

                    case Header.ImageMessage:
                        try
                        {
                            ImageMessage imageMessage = receivedMessage as ImageMessage;
                            Application.Current.Dispatcher.Invoke(new Action(() => { session.LocalMessages.Add(imageMessage); }));
                        }
                        catch { Console.WriteLine("UI Invoke exception caught"); }
                        break;
                }
            }

        }

        //send text message from chat box
        public void sendButtonClick(object sender, EventArgs e)
        {
            TCPHelper.SendTextMessage(session.MessageToSend, null, serverConnection.tcpClient);
        }

        //send image by choosing it
        public void imageButtonClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files(*.BMP; *.JPG;)| *.BMP; *.JPG";

            if (dialog.ShowDialog() == true)
            {
                TCPHelper.SendImageMessage(new System.Drawing.Bitmap(dialog.FileName), null, serverConnection.tcpClient);
            }
        }

        //connect MVVM(?) to window and start listening thread for getting messages
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            session = FindResource("session") as Session;
            Thread thread = new Thread(new ThreadStart(getMessage));
            thread.Start();
        }

        //opening image in full size on click
        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Image caller = sender as Image;

            ImageWindow imageWindow = new ImageWindow(caller.Source);
            imageWindow.Show();
        }
    }

}
