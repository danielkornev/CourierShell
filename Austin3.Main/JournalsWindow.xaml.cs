using System;
using System.Collections.Generic;
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
using ZU.Apps.Austin3.Surfaces;
using ZU.Apps.Austin3.Surfaces.Camera;
using ZU.Apps.Austin3.Surfaces.Journal;

namespace ZU.Apps.Austin3
{
    /// <summary>
    /// Journals Window - Zoomable Canvas with Journals
    /// </summary>
    public partial class JournalsWindow : Window
    {
        JournalsApp journalsAppRightInstance;
        JournalsApp journalsAppLeftInstance;

        CameraPage cameraPageLeftInstance;
        CameraPage cameraPageRightInstance;

        JournalPage journalPageLeftInstance;
        JournalPage journalPageRightInstance;

        public JournalsWindow()
        {
            InitializeComponent();

            this.Loaded += JournalsWindow_Loaded;
        }

        private void JournalsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.ShowActivated = true;

            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.WindowState = WindowState.Maximized;


           
        }

        bool areAppsShown = false;

        

        public CameraPage CameraAppRightInstance
        {
            get
            {
                if (cameraPageRightInstance == null)
                    this.cameraPageRightInstance = new CameraPage();
                return this.cameraPageRightInstance;
            }
        }

        public CameraPage CameraAppLeftInstance
        {
            get
            {
                if (cameraPageLeftInstance == null)
                    this.cameraPageLeftInstance = new CameraPage();
                return this.cameraPageLeftInstance;
            }
        }

        public JournalsApp JournalsAppRightInstance
        {
            get
            {
                if (journalsAppRightInstance == null)
                    this.journalsAppRightInstance = new JournalsApp();
                return this.journalsAppRightInstance;
            }
        }


        public JournalsApp JournalsAppLeftInstance
        {
            get
            {
                if (journalsAppLeftInstance == null)
                    this.journalsAppLeftInstance = new JournalsApp();
                return this.journalsAppLeftInstance;
            }
        }

        public JournalPage JournalPageLeftInstance
        {
            get
            {
                if (journalPageLeftInstance == null)
                    this.journalPageLeftInstance = new JournalPage();
                return this.journalPageLeftInstance;
            }
        }

        public JournalPage JournalPageRightInstance
        {
            get
            {
                if (journalPageRightInstance == null)
                    this.journalPageRightInstance = new JournalPage();
                return this.journalPageRightInstance;
            }
        }

        private void SwitchAppsButton_Click(object sender, RoutedEventArgs e)
        {
            if(areAppsShown)
            {
                // hiding
                HideAppsScroller();
            }
            else
            {
                // showing
                ShowAppsScroller();
            }
        }

        private void ShowAppsScroller()
        {
            this.leftAppsListBox.Visibility = Visibility.Visible;
            leftAppContentPresenter.Visibility = Visibility.Collapsed;

            this.rightAppsListBox.Visibility = Visibility.Visible;
            rightAppContentPresenter.Visibility = Visibility.Collapsed;

            areAppsShown = true;
        }

        private void HideAppsScroller()
        {
            this.leftAppsListBox.Visibility = Visibility.Collapsed;
            leftAppContentPresenter.Visibility = Visibility.Visible;

            this.rightAppsListBox.Visibility = Visibility.Collapsed;
            rightAppContentPresenter.Visibility = Visibility.Visible;

            areAppsShown = false;

        }

        private void LeftAppsListBox_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void JournalAppClicked_LeftSide(object sender, MouseButtonEventArgs e)
        {
            HideAppsScroller();

            leftAppContentPresenter.Visibility = Visibility.Visible;

            leftAppContentPresenter.Content = this.JournalsAppLeftInstance;
        }

        private void JournalAppClicked_RightSide(object sender, MouseButtonEventArgs e)
        {
            HideAppsScroller();

            rightAppContentPresenter.Visibility = Visibility.Visible;

            rightAppContentPresenter.Content = this.JournalsAppRightInstance;
        }


        private void CameraAppClicked_LeftSide(object sender, MouseButtonEventArgs e)
        {
            HideAppsScroller();

            leftAppContentPresenter.Visibility = Visibility.Visible;

            leftAppContentPresenter.Content = this.CameraAppLeftInstance;
        }
        

        private void CameraAppClicked_RightSide(object sender, MouseButtonEventArgs e)
        {
            HideAppsScroller();

            rightAppContentPresenter.Visibility = Visibility.Visible;

            rightAppContentPresenter.Content = this.CameraAppRightInstance;
        }

        private void JournalPageClicked_LeftSide(object sender, MouseButtonEventArgs e)
        {
            HideAppsScroller();

            leftAppContentPresenter.Visibility = Visibility.Visible;
            leftAppContentPresenter.Content = this.JournalPageLeftInstance;
        }

        private void JournalPageClicked_RightSide(object sender, MouseButtonEventArgs e)
        {
            HideAppsScroller();

            rightAppContentPresenter.Visibility = Visibility.Visible;
            rightAppContentPresenter.Content = this.JournalPageRightInstance;
        }
    } // class
} // namespace
