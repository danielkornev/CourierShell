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
using ZU.Apps.Austin3.Storage;

namespace ZU.Apps.Austin3.Surfaces
{
    /// <summary>
    /// Interaction logic for JournalsApp.xaml
    /// </summary>
    public partial class JournalsApp : UserControl
    {
        public JournalsApp()
        {
            InitializeComponent();

            this.Loaded += JournalsApp_Loaded;
        }

        public StorageContext Context { get; private set; }

        private void JournalsApp_Loaded(object sender, RoutedEventArgs e)
        {
            if (StorageContext.JournalsInstance != null)
            {
                this.Context = StorageContext.JournalsInstance;

                // Data Binding
                this.journalsListBox.ItemsSource = this.Context.Journals;
            }
        }

        private void OnManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    } // class
} // namespace
