using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ZU.Shared.Wpf.Controls.Controls
{
    public class ArrangePanel : Panel
    {
        private Duration duration = new Duration(new TimeSpan(0, 0, 0, 0, 400));
        private Random random = new Random();

        public static readonly DependencyProperty ItemSizeProperty =
            DependencyProperty.Register("ItemSize", typeof(int), typeof(ArrangePanel), new FrameworkPropertyMetadata(50, FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register("ItemMargin", typeof(int), typeof(ArrangePanel), new FrameworkPropertyMetadata(5, FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty CrazyModeProperty =
            DependencyProperty.Register("CrazyMode", typeof(bool), typeof(ArrangePanel), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange));

        public int ItemSize
        {
            get { return (int)GetValue(ItemSizeProperty); }
            set { SetValue(ItemSizeProperty, value); }
        }

        public int ItemMargin
        {
            get { return (int)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        public bool CrazyMode
        {
            get { return (bool)GetValue(CrazyModeProperty); }
            set { SetValue(CrazyModeProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            int childrenPerRow = 0;

            foreach (UIElement ele in Children)
            {
                ele.Measure(new Size(ItemSize + ItemMargin, ItemSize + ItemMargin));
            }

            if (availableSize.Width == double.PositiveInfinity)
            {
                childrenPerRow = Children.Count;
            }
            else
            {
                childrenPerRow = Math.Max(1, (int)availableSize.Width / (ItemSize + ItemMargin));
            }

            double totalHeight = this.ItemSize * (Math.Floor((double)this.Children.Count / childrenPerRow) + 1);

            double rowWidth = (ItemSize + ItemMargin) * childrenPerRow;

            return new Size(rowWidth, totalHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Point currentPosition = new Point(ItemMargin, ItemMargin);

            int childrenPerRow = Math.Max(1, (int)finalSize.Width / (ItemSize + ItemMargin));
            int count = 1;

            foreach (FrameworkElement ele in Children)
            {
                // check if should still be on current row
                if (count <= childrenPerRow) // still on same row
                {
                    ArrangeChild(ele, currentPosition, finalSize);
                }
                else // new row
                {
                    currentPosition.Y += ItemSize + ItemMargin; // move position down to new row
                    currentPosition.X = ItemMargin; // reset X position
                    count = 1; // reset count
                    ArrangeChild(ele, currentPosition, finalSize);
                }
                currentPosition.X += ItemSize + ItemMargin; // move position right
                count++;
            }
            return finalSize;
        }

        private void ArrangeChild(FrameworkElement element, Point finalPosition, Size availableSize)
        {
            TranslateTransform trans = element.RenderTransform as TranslateTransform;
            // depending on CrazyMode, we set 0,0 as origin or a random X,Y position on the screen
            double randomX = CrazyMode ? random.Next(0, (int)availableSize.Width) : 0;
            double randomY = CrazyMode ? random.Next(0, (int)availableSize.Height) : 0;
            // create new Translate Transform
            if (trans == null)
            {
                trans = new TranslateTransform();
                element.RenderTransform = trans;
            }
            // tell the child element to arrange itself
            element.Arrange(new Rect(new Point(randomX, randomY), new Size(ItemSize, ItemSize)));

            // keep translation point and store it in it's Tag property for rendering later.
            Point translatePosition = new Point(finalPosition.X - randomX, finalPosition.Y - randomY);

            Dispatcher.BeginInvoke(new Action<FrameworkElement, Point>(BeginAnimation),
                    DispatcherPriority.Render, element, translatePosition);
        }

        private void BeginAnimation(FrameworkElement element, Point translatePosition)
        {
            // hook up X and Y translate transformations
            element.RenderTransform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation(translatePosition.X, duration), HandoffBehavior.Compose);
            element.RenderTransform.BeginAnimation(TranslateTransform.YProperty, new DoubleAnimation(translatePosition.Y, duration), HandoffBehavior.Compose);
        }
    }
}
