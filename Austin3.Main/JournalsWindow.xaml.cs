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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZU.Apps.Austin3.Collections;
using ZU.Apps.Austin3.Storage;
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
        #region Fields
        JournalsApp journalsAppRightInstance;
        JournalsApp journalsAppLeftInstance;

        CameraPage cameraPageInstance;

        GalleryPage galleryAppLeftInstance;
        GalleryPage galleryAppRightInstance;

        bool areAppsShown = false;
        private StorageContext storageContext;
        #endregion

        #region Properties
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

        public GalleryPage GalleryAppRightInstance
        {
            get
            {
                if (galleryAppRightInstance == null)
                    this.galleryAppRightInstance = new GalleryPage(Constants.Side.Right);
                return this.galleryAppRightInstance;
            }
        }

        public GalleryPage GalleryAppLeftInstance
        {
            get
            {
                if (galleryAppLeftInstance == null)
                    this.galleryAppLeftInstance = new GalleryPage(Constants.Side.Left);
                return this.galleryAppLeftInstance;
            }
        }

        public bool IsAppLoaded { get; }
        #endregion

        public JournalsWindow()
        {
            InitializeComponent();

            this.Loaded += JournalsWindow_Loaded;

            this.Closed += JournalsWindow_Closed;

            this.storageContext = new StorageContext();

            // this should always return the last journal
            var lastJournal = this.storageContext.Journals
                .Where(j=>j.IsSoftedDeleted == false)
                .OrderByDescending(j => j.DateUpdated)
                .FirstOrDefault();

            // loading pages
            lastJournal.Pages.Load();

            // updating
            if (lastJournal.Id == Constants.KnownEntities.FirstJournal)
            {
                lastJournal.FrontCoverSolidBrushHex = Constants.Colors.BeautifulGreen;
                lastJournal.FrontCoverSolidColorHex = Constants.Colors.BeautifulGreen;

                // saving changes
                storageContext.SaveJournal(lastJournal);
            }

            // loading journal
            LoadJournal(lastJournal);

            // 
            this.latestJournalsListBox.ItemsSource = this.storageContext.Journals;

            // TouchFrame
            Touch.FrameReported += new TouchFrameEventHandler(Touch_FrameReported);

            this.IsAppLoaded = true;
        }

        

        private void JournalsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.ShowActivated = true;

            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.WindowState = WindowState.Maximized;

            // changing default framerate from 60 to 20
            Timeline.DesiredFrameRateProperty.OverrideMetadata(
               typeof(Timeline),
               new FrameworkPropertyMetadata { DefaultValue = 20 }
               );

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
            //if (this.leftAppContentPresenter.Tag != null)
            //{
                this.leftAppsListBox.Visibility = Visibility.Collapsed;
                leftAppContentPresenter.Visibility = Visibility.Visible;
            //}

            //if (this.rightAppContentPresenter.Tag != null)
            //{
                this.rightAppsListBox.Visibility = Visibility.Collapsed;
                rightAppContentPresenter.Visibility = Visibility.Visible;
            //}

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

            leftAppContentPresenter.Visibility = Visibility.Collapsed;
        }

        private void JournalPageClicked_RightSide(object sender, MouseButtonEventArgs e)
        {
            this.rightAppContentPresenter.Tag = "set";

            HideAppsScroller();

            rightAppContentPresenter.Visibility = Visibility.Collapsed;
        }

        private void GalleryAppClicked_RightSide(object sender, MouseButtonEventArgs e)
        {
            this.rightAppContentPresenter.Tag = "set";

            HideAppsScroller();

            rightAppContentPresenter.Visibility = Visibility.Visible;

            rightAppContentPresenter.Content = this.GalleryAppRightInstance;
        }

        private void GalleryAppClicked_LeftSide(object sender, MouseButtonEventArgs e)
        {
            this.leftAppContentPresenter.Tag = "set";

            HideAppsScroller();

            leftAppContentPresenter.Visibility = Visibility.Visible;

            leftAppContentPresenter.Content = this.GalleryAppLeftInstance;
        }

        private void LatestJournalsListBox_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            // cancelling that inertia thing
            e.Handled = true;
        }

        private void AddNewJournalButton_Click(object sender, RoutedEventArgs e)
        {
            // dirty hack for now
            this.storageContext.CreateJournal("New Journal", Constants.Colors.MidGray);
            // should be fine, for now, again
        }

        private void JournalDataItemGrid_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            var journalEntity = (sender as Grid).DataContext as JournalEntity;
            if (journalEntity == null) return;

            // otherwise, we shall load and open it
            journalEntity.Pages.Load();

            // doing things
            SaveCurrentJournal();

            LoadJournal(journalEntity);


            // animation
            this.BeginStoryboard((Storyboard)this.Resources["zoomToNotebookStoryboard"]);
        }

        private void SaveCurrentJournal()
        {
            JournalEntity journal = GetCurrentJournal();
            journal.DateUpdated = DateTime.UtcNow;
            journal.LastOpenPage = this.currentJournalBook.CurrentSheetIndex;
            this.storageContext.SaveJournal(journal);
        }

        private JournalEntity GetCurrentJournal()
        {
            if (this.currentJournalBook == null) throw new Exception("UI didn't load yet");

            var pages = this.currentJournalBook.ItemsSource as JournalPagesObservableCollection<JournalPageEntity>;
            var journal = pages.Journal;
            return journal;
        }

        private void LoadJournal(JournalEntity journalEntity)
        {
            // binding
            this.currentJournalBook.ItemsSource = journalEntity.Pages;
            this.currentJournalBook.CurrentSheetIndex = journalEntity.LastOpenPage;

            // update current journal's metadata
            UpdateCurrentJournalMetadata(journalEntity);
        }

        private void UpdateCurrentJournalMetadata(JournalEntity journalEntity)
        {
            this.currentJournalDisplayNameTextBox.Text = journalEntity.DisplayName;
            this.currentJournalPagesInfoTextBlock.Text = journalEntity.Pages.Count + " pages";
        }

        private void JournalsWindow_Closed(object sender, EventArgs e)
        {
            // safely saving currently' opened journal
            SaveCurrentJournal();
        }

        private void CurrentJournalDisplayNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsAppLoaded == false) return;

            if (this.currentJournalDisplayNameTextBox.Text.Length>0)
            {
                var currentJournal = GetCurrentJournal();
                currentJournal.DisplayName = this.currentJournalDisplayNameTextBox.Text;

                // saving
                SaveCurrentJournal();
            }
        }
    } // class
} // namespace
