using Austin3.Main.Surfaces.Camera;
using Microsoft.Toolkit.Wpf.UI.XamlHost;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Windows.Devices.Enumeration;
using Windows.Graphics.Display;
using Windows.Media.Capture;
using Windows.System.Display;
using Windows.UI.Core;

namespace ZU.Apps.Austin3.Surfaces.Camera
{
    /// <summary>
    /// Interaction logic for CameraPage.xaml
    /// </summary>
    public partial class CameraPage : UserControl
    {
        public Windows.UI.Xaml.Controls.CaptureElement PreviewControl { get; internal set; }

        StreamPropertiesHelper MaxResolution { get; set; }

        MediaCapture mediaCapture;

        DeviceInformation cameraDevice;

        bool isPreviewing;

        DisplayRequest displayRequest = new DisplayRequest();

        public CameraPage()
        {
            InitializeComponent();

            // required for hosting UWP controls
            global::Windows.UI.Xaml.Hosting.WindowsXamlManager windowsXamlManager = global::Windows.UI.Xaml.Hosting.WindowsXamlManager.InitializeForCurrentThread();

            StartPreviewAsync();
        }

        private void CameraCapturePreview_ChildChanged(object sender, EventArgs e)
        {
            var host = (WindowsXamlHost)sender;
            this.PreviewControl = host.Child as Windows.UI.Xaml.Controls.CaptureElement;
        }

        private async void SetStreamProperties()
        {
            var encodingProperties = MaxResolution.EncodingProperties;
            await mediaCapture.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.VideoPreview, encodingProperties);
        }

        private async Task StartPreviewAsync()
        {
            try
            {
                var devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

                if (devices.Count > 0)
                {
                    cameraDevice = devices.FirstOrDefault();

                    if (devices.Count >= 2)
                    {
                        // Back camera
                        cameraDevice = devices.FirstOrDefault(d => d.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back);
                    }
                }

                var backVideoId = cameraDevice.Id;

                mediaCapture = new MediaCapture();

                await mediaCapture.InitializeAsync(new MediaCaptureInitializationSettings
                {
                    MediaCategory = MediaCategory.Communications,
                    StreamingCaptureMode = StreamingCaptureMode.AudioAndVideo,
                    VideoDeviceId = backVideoId
                });

                displayRequest.RequestActive();
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;

                // doing it
                PopulateStreamProperties(MediaStreamType.VideoPreview, false);

                // setting it
                SetStreamProperties();
            }
            catch (UnauthorizedAccessException)
            {
                // This will be thrown if the user denied access to the camera in privacy settings
                MessageBox.Show("The app was denied access to the camera");
                return;
            }

            try
            {
                PreviewControl.Source = mediaCapture;
                await mediaCapture.StartPreviewAsync();
                isPreviewing = true;
            }
            catch (System.IO.FileLoadException)
            {
                mediaCapture.CaptureDeviceExclusiveControlStatusChanged += _mediaCapture_CaptureDeviceExclusiveControlStatusChanged;
            }

        }

        private async Task CleanupCameraAsync()
        {
            if (mediaCapture != null)
            {
                if (isPreviewing)
                {
                    await mediaCapture.StopPreviewAsync();
                }

                //await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                //{
                //    PreviewControl.Source = null;
                //    if (displayRequest != null)
                //    {
                //        displayRequest.RequestRelease();
                //    }

                //    mediaCapture.Dispose();
                //    mediaCapture = null;
                //});
            }

        }

        private async void _mediaCapture_CaptureDeviceExclusiveControlStatusChanged(MediaCapture sender, MediaCaptureDeviceExclusiveControlStatusChangedEventArgs args)
        {
            if (args.Status == MediaCaptureDeviceExclusiveControlStatus.SharedReadOnlyAvailable)
            {
                MessageBox.Show("The camera preview can't be displayed because another app has exclusive access");
            }
            else if (args.Status == MediaCaptureDeviceExclusiveControlStatus.ExclusiveControlAvailable && !isPreviewing)
            {
                //await Dispatcher.Run(CoreDispatcherPriority.Normal, async () =>
                //{
                //    await StartPreviewAsync();
                //});
            }
        }

        private void PopulateStreamProperties(MediaStreamType streamType, bool showFrameRate = true)
        {
            // Query all properties of the specified stream type 
            IEnumerable<StreamPropertiesHelper> allStreamProperties =
                mediaCapture.VideoDeviceController.GetAvailableMediaStreamProperties(streamType).Select(x => new StreamPropertiesHelper(x));

            // Order them by resolution then frame rate
            allStreamProperties = allStreamProperties.OrderByDescending(x => x.Height * x.Width).ThenByDescending(x => x.FrameRate);

            // Max Resolution
            MaxResolution = allStreamProperties.FirstOrDefault();
        }

        private void SomeButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    } // class
} // namespace
