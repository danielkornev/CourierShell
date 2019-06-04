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
        private InkAnalyzer newAnalyzer;

        public JournalPage()
        {
            InitializeComponent();

            this.Loaded += JournalPage_Loaded;

            this.inkCanvas.Loaded += InkCanvas_Loaded;
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
            newAnalyzer.AddStroke(e.Stroke);
            newAnalyzer.BackgroundAnalyze();
        }

        private void newAnalyzer_ResultsUpdated(object sender, ResultsUpdatedEventArgs e)
        {
            //
        }

        private void JournalPage_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    } // class
} // namespace
