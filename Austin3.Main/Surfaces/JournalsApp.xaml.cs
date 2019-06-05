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

        private void JournalsApp_Loaded(object sender, RoutedEventArgs e)
        {
            var n1 = new JournalEntity
            {
                FrontCoverBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(Constants.Colors.Violet)),
                FrontCoverColor = (Color)ColorConverter.ConvertFromString(Constants.Colors.Violet),
                DisplayName = "Space Travel",
                Width = 205,
                X = 350,
                Y = 490
            };

            var n2 = new JournalEntity
            {
                FrontCoverBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(Constants.Colors.LightBlue)),
                FrontCoverColor = (Color)ColorConverter.ConvertFromString(Constants.Colors.LightBlue),
                DisplayName = "Short Stories",
                Width = 185,
                X = 80,
                Y = 129
            };

            var n3 = new JournalEntity
            {
                FrontCoverBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(Constants.Colors.BeautifulGreen)),
                FrontCoverColor = (Color)ColorConverter.ConvertFromString(Constants.Colors.BeautifulGreen),
                DisplayName = "Meeting Notes",
                Width = 300,
                X = 300,
                Y = 100
            };

            var n4 = new JournalEntity
            {
                FrontCoverBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(Constants.Colors.KingBrown)),
                FrontCoverColor = (Color)ColorConverter.ConvertFromString(Constants.Colors.KingBrown),
                DisplayName = "Notes",
                Width = 150,
                X = 650,
                Y = 150
            };


            this.journalsListBox.Items.Add(n1);
            this.journalsListBox.Items.Add(n2);
            this.journalsListBox.Items.Add(n3);
            this.journalsListBox.Items.Add(n4);
        }

        private void OnManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    } // class
} // namespace
