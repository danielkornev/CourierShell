using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using System.Windows.Threading;

namespace Austin3.Main.Objects
{
    /// <summary>
    /// Interaction logic for GalleryPhoto.xaml
    /// </summary>
    public partial class GalleryPhoto : UserControl
    {
        #region Fields
        private bool isSelected;
        private DispatcherTimer picTimer;
        private int waitTime;

        #endregion

        public FileInfo ImageFile { get; set; }

        public GalleryPhoto()
        {
            InitializeComponent();

            isSelected = false;

            // or 0
            waitTime = 1;

            picTimer = new DispatcherTimer();

            picTimer.Interval = new TimeSpan(0, 0, 0, 0, 100 * waitTime);
            picTimer.IsEnabled = true;
            picTimer.Start();
            picTimer.Tick += PicTimer_Tick;
        }

        private void PicTimer_Tick(object sender, EventArgs e)
        {
            fileIcon.Visibility = Visibility.Collapsed;
            picview.Visibility = Visibility.Visible;
            System.Drawing.Image image = System.Drawing.Image.FromFile(ImageFile.FullName);
            loadIMG.Width = (double)image.PhysicalDimension.Width;
            loadIMG.Height = (double)image.PhysicalDimension.Height;
            BitmapImage bitmapImage = new BitmapImage();
            MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(ImageFile.FullName));
            bitmapImage.BeginInit();
            bitmapImage.DecodePixelHeight = 240;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();
            memoryStream.Dispose();
            loadIMG.Source = bitmapImage;
            memoryStream = null;
            bitmapImage = null;
        }

        private void loadIMG_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            picview.Width = loadIMG.ActualWidth + 2.0;
            picview.Height = loadIMG.ActualHeight + 2.0;
        }

        private void LayoutRoot_MouseEnter(object sender, MouseEventArgs e)
        {
            //if (!isSelected)
            //{
            //    rectangle.Opacity = 0.5;
            //}
        }

        private void LayoutRoot_MouseLeave(object sender, MouseEventArgs e)
        {
            //if (!isSelected)
            //{
            //    rectangle.Opacity = 0.0;
            //}
        }

        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //rectangle.Opacity = 1.0;
        }

        private void dragMe(object sender, MouseEventArgs e)
        {

        }

        private void killDrag(object sender, MouseButtonEventArgs e)
        {

        }

        
    } // class
} // namespace
