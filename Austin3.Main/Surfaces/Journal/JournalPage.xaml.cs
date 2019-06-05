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

namespace ZU.Apps.Austin3.Surfaces.Journal
{
    /// <summary>
    /// Interaction logic for JournalPage.xaml
    /// </summary>
    public partial class JournalPage : UserControl
    {
        bool isPageLoaded = false;

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

            
        }

        private void InkCanvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            try
            {
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

            AdaptToSide(this.PageSide);
        }

        private void InkCanvas_TouchDown(object sender, TouchEventArgs e)
        {
            e.Handled = true;
        }

        private void InkCanvas_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            e.Handled = true;
        }

        private void InkCanvas_PreviewTouchMove(object sender, TouchEventArgs e)
        {
            e.Handled = true;
        }

        private void InkCanvas_PreviewStylusDown(object sender, StylusDownEventArgs e)
        {
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
                e.Handled = true;
        }

        private void InkCanvas_PreviewStylusMove(object sender, StylusEventArgs e)
        {
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
                e.Handled = true;
        }
    } // class
} // namespace
