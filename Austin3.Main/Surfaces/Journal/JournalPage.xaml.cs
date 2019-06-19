using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZU.Apps.Austin3.Storage;
using ZU.Shared.Wpf.Controls;
using ZU.Shared.Wpf.Controls.Controls;
using ZU.Shared.Wpf.Ink;

namespace ZU.Apps.Austin3.Surfaces.Journal
{
    /// <summary>
    /// Interaction logic for JournalPage.xaml
    /// </summary>
    public partial class JournalPage : UserControl
    {
        bool isPageLoaded = false;

        bool isInkToolbarShown = false;

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
                    inkToolsBorder.HorizontalAlignment = HorizontalAlignment.Right;
                    inkToolsBorder.CornerRadius = new CornerRadius(5, 0, 0, 5);
                    inkToolsBorder.FlowDirection = FlowDirection.RightToLeft;
                    bookCenterEffectGrid.FlowDirection = FlowDirection.RightToLeft;
                    bookCenterEffectGrid.HorizontalAlignment = HorizontalAlignment.Right;
                    pageBorder.BorderThickness = new Thickness(1, 1, 0, 1);
                    inkToolsGrid.Width = 0;
                    if (inkToolsGrid.Children.Count == 0)
                    {
                        inkToolsGrid.Children.Add(new InkToolbarControl());
                    }
                    (inkToolsBorder.RenderTransform as TransformGroup).Children.OfType<TranslateTransform>().First().X = 400;

                    break;
                case Constants.Side.Right:
                    pageNumberAndOptionsGrid.HorizontalAlignment = HorizontalAlignment.Right;
                    inkToolsBorder.HorizontalAlignment = HorizontalAlignment.Left;
                    inkToolsBorder.CornerRadius = new CornerRadius(0, 5, 5, 0);
                    inkToolsBorder.FlowDirection = FlowDirection.LeftToRight;
                    bookCenterEffectGrid.FlowDirection = FlowDirection.LeftToRight;
                    bookCenterEffectGrid.HorizontalAlignment = HorizontalAlignment.Left;
                    pageBorder.BorderThickness = new Thickness(0, 1, 1, 1);
                    inkToolsGrid.Width = 0;
                    if (inkToolsGrid.Children.Count == 0)
                    {
                        inkToolsGrid.Children.Add(new InkToolbarControl());
                    }
                    (inkToolsBorder.RenderTransform as TransformGroup).Children.OfType<TranslateTransform>().First().X = -400;

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

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                var parent = Application.Current.MainWindow as JournalsWindow;


                if (parent == null) throw new Exception("Main Window is not a Journals Window!");

                parent.InkDrawingAttributesChanged += Parent_InkDrawingAttributesChanged;
            }
        }

        private void Parent_InkDrawingAttributesChanged(object sender, DrawingAttributesChangedEventArgs e)
        {
            if (e.DrawingAttributes!=null)
            {
                // updating our drawing attributes
                this.inkCanvas.DefaultDrawingAttributes = e.DrawingAttributes;
            }
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

            // Page Side
            if (this.Context == null) return;

            if (this.Context.IsOdd) this.PageSide = Constants.Side.Right;
            else this.PageSide = Constants.Side.Left;

        }

        private void InkCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.StylusDevice == null) return;
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
            {
                this.pageNumberAndOptionsGrid.Visibility = Visibility.Visible;


                var bookPage = this.FindParent<BookPage>();
                if (bookPage!=null)
                {
                    bookPage.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, e.Timestamp, e.ChangedButton, e.StylusDevice)
                    {
                        RoutedEvent = Mouse.MouseDownEvent
                    });
                }

                e.Handled = true;
            }
        }

        private void InkCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.StylusDevice == null) return;
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
            {
                var bookPage = this.FindParent<BookPage>();
                if (bookPage != null)
                {
                    bookPage.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, e.Timestamp, e.StylusDevice)
                    {
                        RoutedEvent = Mouse.MouseMoveEvent
                    });
                }

                e.Handled = true;
            }
        }

        private void InkCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.StylusDevice == null) return;
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Touch)
            {
                var bookPage = this.FindParent<BookPage>();
                if (bookPage != null)
                {
                    bookPage.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, e.Timestamp, MouseButton.Left, e.StylusDevice)
                    {
                        RoutedEvent = Mouse.MouseUpEvent
                    });
                }

                e.Handled = true;
            }
        }

        //private Book GetBookControl()
        //{
        //    // obtaining Journals Window
        //    var mainWindow = Application.Current.MainWindow as JournalsWindow;

        //    return mainWindow.currentJournalBook;
        //}

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

        private void InkCanvas_PreviewStylusInRange(object sender, StylusEventArgs e)
        {
            this.pageNumberAndOptionsGrid.Visibility = Visibility.Hidden;
        }

        private void InkCanvas_PreviewStylusUp(object sender, StylusEventArgs e)
        {

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

            if (Context.IsCoverPage)
            {
                // color
                this.frontCoverGrid.Background = Context.Journal.FrontCoverBrush;

                // name
                this.journalDisplayNameTextBlock.Text = Context.Journal.DisplayName;

                // # of pages
                this.journalPagesTextBlock.Text = Context.Journal.Pages.Count.ToString() + " pages";

                this.frontCoverGrid.Opacity = 1.0;
            }
            else
            {
                this.frontCoverGrid.Opacity = 0.0;
            }

            if (Context.InkLayer != null)
            {
                // obtaining ink layer from the entity
                this.inkCanvas.Strokes = Context.InkLayer;
            }


            // Page Side
            if (this.Context.IsOdd) this.PageSide = Constants.Side.Right;
            else this.PageSide = Constants.Side.Left;

            AdaptToSide(this.PageSide);

            // preparing ink toolbar
            var parent = Application.Current.MainWindow as JournalsWindow;
            if (parent == null) throw new Exception("Main Window is not a Journals Window!");

            if (parent.InkDrawingAttributes != null)
            {
                this.inkCanvas.DefaultDrawingAttributes = parent.InkDrawingAttributes;

                if (this.inkToolsGrid.Children.Count==1)
                {
                    var inkToolbarControl = this.inkToolsGrid.Children.OfType<InkToolbarControl>().First();
                    
                    inkToolbarControl.SetSelectedColor(parent.InkDrawingAttributes.Color);
                }
            }

            // hiding ink toolbar
            switch (this.PageSide)
            {
                case Constants.Side.Left:
                    if (this.isInkToolbarShown)
                    {
                        this.BeginStoryboard((Storyboard)this.Resources["collapseLeftInkToolBarStoryBoard"]);

                        this.isInkToolbarShown = false;
                    }

                    break;
                case Constants.Side.Right:
                    if (this.isInkToolbarShown)
                    {
                        this.BeginStoryboard((Storyboard)this.Resources["collapseRightInkToolBarStoryBoard"]);

                        this.isInkToolbarShown = false;
                    }

                    break;
                case Constants.Side.Unknown:
                    break;
                default:
                    break;
            }

            // Page Number & Options 
            this.pageNumberAndOptionsGrid.Visibility = Visibility.Hidden;

            // using page's Id as it's number
            this.PageNumber = this.Context.Id;
        }

        private void InkToolbarGrid_TouchDown(object sender, TouchEventArgs e)
        {
            switch (this.PageSide)
            {
                case Constants.Side.Left:
                    if (this.isInkToolbarShown == false)
                    {
                        // we begin animation
                        this.BeginStoryboard((Storyboard)this.Resources["expandLeftInkToolBarStoryBoard"]);

                        this.isInkToolbarShown = true;
                    }
                    else
                    {
                        this.BeginStoryboard((Storyboard)this.Resources["collapseLeftInkToolBarStoryBoard"]);

                        this.isInkToolbarShown = false;
                    }

                    break;
                case Constants.Side.Right:
                    if (this.isInkToolbarShown == false)
                    {
                        // we begin animation
                        this.BeginStoryboard((Storyboard)this.Resources["expandRightInkToolBarStoryBoard"]);

                        this.isInkToolbarShown = true;
                    }
                    else
                    {
                        this.BeginStoryboard((Storyboard)this.Resources["collapseRightInkToolBarStoryBoard"]);

                        this.isInkToolbarShown = false;
                    }

                    break;
                case Constants.Side.Unknown:
                    break;
                default:
                    break;
            }


        }

        private void InkCanvas_PreviewStylusOutOfRange(object sender, StylusEventArgs e)
        {
            // disabling hit test on ink canvas
            this.inkCanvas.IsHitTestVisible = false;
        }

        private void NormalPageGrid_PreviewStylusInRange(object sender, StylusEventArgs e)
        {
            if (e.StylusDevice.TabletDevice.Type == TabletDeviceType.Stylus)
            {
                this.inkCanvas.IsHitTestVisible = true;
            }
        }
    } // class
} // namespace
