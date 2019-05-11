using System;
using System.Net;
using System.Windows;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для ConnectWindow.xaml
    /// </summary>    
    public partial class ConnectWindow : Window
    {
        private IPAddress IpAddress;
        private int port;
        private Connection connection;
      
        public ConnectWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(IPAddress.TryParse(IPBox.Text, out IpAddress) && int.TryParse(PortBox.Text,out port) && NameBox.Text!=String.Empty)
            {
                connection = new Connection(new IPEndPoint(IpAddress,port));
                if(connection.Autenficate(NameBox.Text))
                {
                    MainWindow mainWindow = new MainWindow(connection);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Name is taken");
                    connection.tcpClient.Close();
                }               
            }
            else
            {
                MessageBox.Show("Incorrect data");
            }
        }
    }
}
