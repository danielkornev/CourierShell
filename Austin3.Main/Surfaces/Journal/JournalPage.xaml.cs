using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZU.Apps.Austin3.Storage;
using ZU.Shared.Wpf.Ink;

namespace ZU.Apps.Austin3.Surfaces.Journal
{
    /// <summary>
    /// Interaction logic for JournalPage.xaml
    /// </summary>
    public partial class JournalPage : UserControl
    {
        bool isPageLoaded = false;

        public JournalPageEntity Context
        {
            get; internal set;
        }

        public Constants.Side PageSide
        {
            get { return (Constants.Side)GetValue(PageSideProperty); }
            set { SetValue(PageSideProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageSide.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSideProperty =
            DependencyProperty.Register("PageSide", typeof(Constants.Side), typeof(JournalPage), new PropertyMetadata(Constants.Side.Unknown, SideChanged));

        private static void SideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            JournalPage page = d as JournalPage;
            page.OnSideChanged(e, page.PageSide);
        }

        private void OnSideChanged(DependencyPropertyChangedEventArgs e, Constants.Side pageSide)
        {
            if (isPageLoaded == false)
                return;

            AdaptToSide(pageSide);
        }

        public int PageNumber
        {
            get { return (int)GetValue(PageNumberProperty); }
            set { SetValue(PageNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageNumberProperty =
            DependencyProperty.Register("PageNumber", typeof(int), typeof(JournalPage), new PropertyMetadata(1));

        private void AdaptToSide(Constants.Side pageSide)
        {
            switch (pageSide)
            {
                case Constants.Side.Left:
                    pageNumberAndOptionsGrid.HorizontalAlignment = HorizontalAlignment.Left;

                    break;
                case Constants.Side.Right:
                    pageNumberAndOptionsGrid.HorizontalAlignment = HorizontalAlignment.Right;

                    break;
                case Constants.Side.Unknown:
                    break;
                default:
                    break;
            }
        }

        private InkAnalyzer newAnalyzer;

        public JournalPage()
        {
            InitializeComponent();

            // initializing platform
            PlatformWpf.Init(this);

            this.Loaded += JournalPage_Loaded;

            this.inkCanvas.Loaded += InkCanvas_Loaded;
        }

        public JournalPage(Constants.Side side, int pageNumber) : this()
        {
            this.PageSide = side;
            this.PageNumber = pageNumber;
        }

        private void InkCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            newAnalyzer = new InkAnalyzer
            {
                AnalysisModes = AnalysisModes.AutomaticReconciliationEnabled
            };
            newAnalyzer.ResultsUpdated += new ResultsUpdatedEventHandler(newAnalyzer_ResultsUpdated);

            this.inkCanvas.StrokeCollected += InkCanvas_StrokeCollected;
            this.inkCanvas.StrokeErased += InkCanvas_StrokeErased;

            //this.inkCanvas.PreviewTouchUp += InkCanvas_PreviewTouchUp;
        }

        private void InkCanvas_StrokeErased(object sender, RoutedEventArgs e)
        {
            this.pageNumberAndOptionsGrid.Visibility = Visibility.Hidden;

            try
            {
                // saving
                Context.InkStrokes =
                    InkStrokeCollectionConverter.InternalInstance.ConvertToString(this.inkCanvas.Strokes);

                // saving
                Context.StorageContext.SavePage(Context);
            }

            catch
            {

            }
        }

        private void InkCanvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            try
            {
                // saving
                Context.InkStrokes = 
                    InkStrokeCollectionConverter.InternalInstance.ConvertToString(this.inkCanvas.Strokes);

                // saving
                Context.StorageContext.SavePage(Context);

                // checking if it's the last page or not
                if (this.Context == Context.Journal.Pages.Last())
                {
                    // this is the last page
                    // we shall add two more pages
                    Context.Journal.AddTwoMorePages();
                }

                newAnalyzer.AddStroke(e.Stroke);
                newAnalyzer.BackgroundAnalyze();
            }
            catch
            {
                // we get to Exception "At least one stroke in the collection is already referenced by the InkAnalyzer" somehow. No idea why.


            }
        }

        private void newAnalyzer_ResultsUpdated(object sender, ResultsUpdatedEventArgs e)
        {
            //
        }

        private void JournalPage_Loaded(object sender, RoutedEventArgs e)
        {
            isPageLoaded = true;
        }

        private void InkCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.StylusDevice == null) return;
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
            {
                this.pageNumberAndOptionsGrid.Visibility = Visibility.Visible;

                e.Handled = true;
            }
        }

        private void InkCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.StylusDevice == null) return;
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
            {
                e.Handled = true;
            }
        }

        private void InkCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.StylusDevice == null) return;
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
            {
               

                e.Handled = true;
            }
        }

        private void InkCanvas_PreviewStylusDown(object sender, StylusDownEventArgs e)
        {
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
                e.Handled = true;

            // hiding then
            this.pageNumberAndOptionsGrid.Visibility = Visibility.Hidden;
        }

        private void InkCanvas_PreviewStylusMove(object sender, StylusEventArgs e)
        {
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
                e.Handled = true;
        }

        private void JournalPage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.pageNumberAndOptionsGrid.Visibility = Visibility.Collapsed;

            // clearing Ink Layer
            this.inkCanvas.Strokes.Clear();

            //
            var entity = this.DataContext as JournalPageEntity;

            // but this is simply strange
            if (entity == null)
            {
                var canvas = this.DataContext as System.Windows.Controls.Canvas;
                if (canvas!=null)
                {

                    
                }

                return;
            }

            this.Context = entity;

            if (Context.IsCoverPage) return;

            if (Context.InkLayer != null)
            {
                // obtaining ink layer from the entity
                this.inkCanvas.Strokes = Context.InkLayer;
            }


            // Page Side
            if (this.Context.IsOdd) this.PageSide = Constants.Side.Right;
            else this.PageSide = Constants.Side.Left;

            // 
            this.pageNumberAndOptionsGrid.Visibility = Visibility.Hidden;

            // using page's Id as it's number
            this.PageNumber = this.Context.Id;
        }

        private void InkCanvas_PreviewStylusInRange(object sender, StylusEventArgs e)
        {
            this.pageNumberAndOptionsGrid.Visibility = Visibility.Hidden;
        }
    } // class
} // namespace
