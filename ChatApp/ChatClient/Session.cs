using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TCPTalker;


namespace ChatClient
{
    // a small class to store values binded to UI
    class Session : INotifyPropertyChanged
    {
        private string messageToSend;
        public string MessageToSend
        {
            get { return messageToSend; }
            set { messageToSend = value; OnPropertyChange("MessageToSend"); }
        }

        
        public ObservableCollection<Message> LocalMessages { get; set; }

        public Session()
        {
            LocalMessages = new ObservableCollection<Message>() { new TextMessage(DateTime.Now, "Server", "Connected to server") };
            MessageToSend = "Enter your message";
        }

        private void OnPropertyChange(string propertyName)
        {
            if(PropertyChanged!=null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
