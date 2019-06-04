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

        CameraPage cameraPageInstance;

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

        

        public CameraPage CameraAppInstance
        {
            get
            {
                if (cameraPageInstance == null)
                    this.cameraPageInstance = new CameraPage();
                return this.cameraPageInstance;
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
                    this.journalPageLeftInstance = new JournalPage(Constants.Side.Left, 1);
                return this.journalPageLeftInstance;
            }
        }

        public JournalPage JournalPageRightInstance
        {
            get
            {
                if (journalPageRightInstance == null)
                    this.journalPageRightInstance = new JournalPage(Constants.Side.Right, 1);
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
            if (this.leftAppContentPresenter.Tag != null)
            {
                this.leftAppsListBox.Visibility = Visibility.Collapsed;
                leftAppContentPresenter.Visibility = Visibility.Visible;
            }

            if (this.rightAppContentPresenter.Tag != null)
            {
                this.rightAppsListBox.Visibility = Visibility.Collapsed;
                rightAppContentPresenter.Visibility = Visibility.Visible;
            }

            areAppsShown = false;

        }

        private void LeftAppsListBox_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void JournalAppClicked_LeftSide(object sender, MouseButtonEventArgs e)
        {
            this.leftAppContentPresenter.Tag = "set";

            HideAppsScroller();

            leftAppContentPresenter.Visibility = Visibility.Visible;

            leftAppContentPresenter.Content = this.JournalsAppLeftInstance;
        }

        private void JournalAppClicked_RightSide(object sender, MouseButtonEventArgs e)
        {
            this.rightAppContentPresenter.Tag = "set";

            HideAppsScroller();

            rightAppContentPresenter.Visibility = Visibility.Visible;

            rightAppContentPresenter.Content = this.JournalsAppRightInstance;
        }


        private void CameraAppClicked_LeftSide(object sender, MouseButtonEventArgs e)
        {
            this.leftAppContentPresenter.Tag = "set";

            HideAppsScroller();

            leftAppContentPresenter.Visibility = Visibility.Visible;

            if (rightAppContentPresenter.Content == this.CameraAppInstance)
                rightAppContentPresenter.Content = null;

            leftAppContentPresenter.Content = this.CameraAppInstance;
        }
        

        private void CameraAppClicked_RightSide(object sender, MouseButtonEventArgs e)
        {
            this.rightAppContentPresenter.Tag = "set";

            HideAppsScroller();

            rightAppContentPresenter.Visibility = Visibility.Visible;

            if (leftAppContentPresenter.Content == this.CameraAppInstance)
                leftAppContentPresenter.Content = null;

            rightAppContentPresenter.Content = this.CameraAppInstance;
        }

        private void JournalPageClicked_LeftSide(object sender, MouseButtonEventArgs e)
        {
            this.leftAppContentPresenter.Tag = "set";

            HideAppsScroller();

            leftAppContentPresenter.Visibility = Visibility.Visible;
            leftAppContentPresenter.Content = this.JournalPageLeftInstance;

            if (rightAppContentPresenter.Content == this.journalPageRightInstance)
            {
                this.JournalPageRightInstance.PageNumber = 2;
            }
        }

        private void JournalPageClicked_RightSide(object sender, MouseButtonEventArgs e)
        {
            this.rightAppContentPresenter.Tag = "set";

            HideAppsScroller();

            rightAppContentPresenter.Visibility = Visibility.Visible;
            rightAppContentPresenter.Content = this.JournalPageRightInstance;

            if (leftAppContentPresenter.Content == this.journalPageLeftInstance)
            {
                this.JournalPageRightInstance.PageNumber = 2;
            }
        }
    } // class
} // namespace
