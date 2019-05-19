using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TCPTalker;

namespace ChatClient
{
    /// <summary>
    /// small window to display image in full size
    /// </summary>
    public partial class ImageWindow : Window
    {

        ImageSource Image;

        public ImageWindow(ImageSource image)
        {
            InitializeComponent();           
            this.Image = image;
            ImageView.Source = Image;
        }

        /// <summary>
        /// Click on image to close it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}

