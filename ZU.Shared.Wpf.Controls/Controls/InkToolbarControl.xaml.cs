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

namespace ZU.Shared.Wpf.Controls.Controls
{
    /// <summary>
    /// Interaction logic for InkToolbarControl.xaml
    /// </summary>
    public partial class InkToolbarControl : UserControl
    {
        public InkToolbarControl()
        {
            InitializeComponent();

            this.Loaded += InkToolbarControl_Loaded;

            this.colorsListBox.SelectionChanged += ColorsListBox_SelectionChanged;
        }

        private void ColorsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.colorsListBox.SelectedIndex!=-1)
            {
                var colorBrush = this.colorsListBox.SelectedItem as SolidColorBrush;
                if (colorBrush!=null)
                {
                    dynamic parent = Application.Current.MainWindow;

                    DrawingAttributes inkAttributes = parent.InkDrawingAttributes;

                    if (inkAttributes == null)
                        inkAttributes = new DrawingAttributes();

                    // setting values
                    parent.InkDrawingAttributes = new DrawingAttributes
                    {
                        Color = colorBrush.Color
                    };
                    
                }
            }
        }

        private void InkToolbarControl_Loaded(object sender, RoutedEventArgs e)
        {
            var colors = new List<SolidColorBrush>()
            {
                new SolidColorBrush(Colors.Black),
                new SolidColorBrush(Colors.White),
                new SolidColorBrush(Colors.Red),
                new SolidColorBrush(Colors.DarkOrange),
                new SolidColorBrush(Colors.Yellow),
                new SolidColorBrush(Colors.LawnGreen),
                new SolidColorBrush(Colors.Blue),
                new SolidColorBrush(Colors.Purple),
                new SolidColorBrush(Colors.DeepPink),
                new SolidColorBrush(Colors.Aqua),
                new SolidColorBrush(Colors.SaddleBrown),
                new SolidColorBrush(Colors.Wheat),
                new SolidColorBrush(Colors.BurlyWood),
                new SolidColorBrush(Colors.Teal),
                new SolidColorBrush(Colors.OrangeRed),
                new SolidColorBrush(Colors.Orange),
                new SolidColorBrush(Colors.Gold),
                new SolidColorBrush(Colors.LimeGreen)
            };

            // binding
            this.colorsListBox.ItemsSource = colors;
        }

        public void SetSelectedColor(Color color)
        {
            var colors = this.colorsListBox.Items;
            if (colors!=null)
            {
                var fcIndex = this.colorsListBox.Items.IndexOf(new SolidColorBrush(color));

                if (fcIndex>-1)
                {

                }

                //var foundColors = colors.OfType<ListBoxItem>().Where(c => (c.DataContext as SolidColorBrush).Color == color).ToList();
                //if (foundColors!=null)
                //{
                //    var foundColor = foundColors.First();

                //    // setting as selected
                //    this.colorsListBox.SelectedItem = foundColor;
                //}
            }
        }
    } // class
} // namespace
