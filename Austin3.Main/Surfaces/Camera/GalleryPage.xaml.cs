using Austin3.Main.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.Storage;
using ZU.Shared.Wpf.Media;

namespace ZU.Apps.Austin3.Surfaces.Camera
{
    /// <summary>
    /// Interaction logic for GalleryPage.xaml
    /// </summary>
    public partial class GalleryPage : UserControl
    {
        BitmapImageCheck bic;

        ObservableCollection<GalleryPhoto> GalleryPhotos = new ObservableCollection<GalleryPhoto>();

        public GalleryPage()
        {
            bic = new BitmapImageCheck();

            InitializeComponent();

            BindToPicturesFolder();
        }

        private async void BindToPicturesFolder()
        {
            var myPictures = await StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Pictures);

            var defaultSaveFolder = myPictures.SaveFolder;
            // creating or getting Camera folder
            var austinCameraFolder = await defaultSaveFolder.CreateFolderAsync("Camera", CreationCollisionOption.OpenIfExists);

            // path
            var cameraDirectory = new System.IO.DirectoryInfo(
                austinCameraFolder.Path);


            // obtaining files
            var items = cameraDirectory.GetFiles().ToList();

            // this is super inefficient, but should enough for demo
            foreach (var item in items)
            {
                var path = item.FullName;
                if (bic.IsExtensionSupported(path))
                {
                    BitmapImage myImageSource = new BitmapImage();
                    myImageSource.BeginInit();
                    myImageSource.UriSource = new Uri(path);
                    myImageSource.EndInit();

                    myImageSource.Freeze();

                    GalleryPhotos.Add(new GalleryPhoto
                    {
                        Image = myImageSource,
                        ImageFile = item
                    });
                }
            }

            // showing (this is super inefficient, I know)
            foreach (var item in GalleryPhotos)
            {
                this.photoGalleryPanel.Children.Add(item);
            }
        }

        public GalleryPage(Constants.Side pageSide) : this()
        {
            // switching direction of controls
            if (pageSide == Constants.Side.Right)
                this.controlsAreaStackPanel.FlowDirection = FlowDirection.LeftToRight;
        }
    } // class
} // namespace
