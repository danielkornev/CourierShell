using Austin3.Main.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        FileSystemWatcher watcher;

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

            // adding FileSystemWatcher
            watcher = new FileSystemWatcher(cameraDirectory.FullName);
            watcher.Created += Watcher_Created;
            watcher.Renamed += Watcher_Renamed;
            watcher.Deleted += Watcher_Deleted;
            watcher.Changed += Watcher_Changed;

            // enabling folder watching
            watcher.EnableRaisingEvents = true;

            // how to identify if contents changed?

            // obtaining files
            var items = cameraDirectory.GetFiles().ToList();

            // this is super inefficient, but should be good enough for demo
            foreach (var item in items)
            {
                var path = item.FullName;
                if (bic.IsExtensionSupported(path))
                {
                    GalleryPhotos.Add(new GalleryPhoto
                    {
                        ImageFile = item
                    });
                }
            }

            // showing (this is super inefficient, I know)
            foreach (var item in GalleryPhotos)
            {
                this.photoGalleryPanel.Items.Add(item);
            }
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (bic.IsExtensionSupported(e.FullPath) == false) return;

            // TODO
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (bic.IsExtensionSupported(e.FullPath) == false) return;

            try
            {
                var samePhotos = this.GalleryPhotos.Where(i => i.ImageFile.FullName == e.FullPath).ToList();
                if (samePhotos.Count() == 1)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        var item = samePhotos.First();

                        this.photoGalleryPanel.Items.Remove(item);

                        this.GalleryPhotos.Remove(item);
                    });
                }
            }
            catch
            {

            }
           
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            if (bic.IsExtensionSupported(e.FullPath) == false) return;

            if (System.IO.File.Exists(e.FullPath) == false) return;

            // this is a bit harder; we should find it and change an image;
            // TODO
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (bic.IsExtensionSupported(e.FullPath) == false) return;

            if (System.IO.File.Exists(e.FullPath) == false) return;

            try
            {
                var file = new FileInfo(e.FullPath);

                var samePhotos = this.GalleryPhotos.Where(i => i.ImageFile.FullName == file.FullName).ToList();
                if (samePhotos.Count() == 0)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.GalleryPhotos.Add(new GalleryPhoto
                        {
                            ImageFile = file
                        });

                        this.photoGalleryPanel.Items.Add(new GalleryPhoto
                        {
                            ImageFile = file
                        });
                    });
                }
            }
            catch
            {

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
