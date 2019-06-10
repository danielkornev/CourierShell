using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ZU.Apps.Austin3
{
    public partial class JournalsWindow
    {
        private int numTouchPoints;

        public double TouchZoomDist { get; private set; }
        public double TouchZoomScale { get; private set; }
        public Point TouchZoomCenter { get; private set; }

        #region Helper Methods
        public static double Distance(Point p, Point q)
        {
            double a = p.X - q.X;
            double b = p.Y - q.Y;
            double distance = Math.Sqrt(a * a + b * b);
            return distance;
        }

        public static Point Lerp(Point p1, Point p2, double amount)
        {
            double x = (1 - amount) * (p1.X) + amount * (p2.X);
            double y = (1 - amount) * (p1.Y) + amount * (p2.Y);
            return new Point(x, y);
        }
        #endregion

        private void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            var points = e.GetTouchPoints(bigGrid);
            numTouchPoints = points.Count;

            if (numTouchPoints > 0)
            {
                // do nothing, for now
            }

            if (numTouchPoints == 2)
            {
                // Scale
                var p1 = points[0];
                var p2 = points[1];

                // ScaleTransform
                var scaleTransform = bigGrid.RenderTransform as ScaleTransform;
                if (scaleTransform == null) return; // for now

                //
                if ((p1.Action == TouchAction.Down) || (p2.Action == TouchAction.Down))
                {
                    // Start zoom
                    TouchZoomDist = Distance(p1.Position, p2.Position);
                    TouchZoomScale = scaleTransform.ScaleX;
                    TouchZoomCenter = Lerp(p1.Position, p2.Position, 0.5);
                    return;
                }
                if ((p1.Action == TouchAction.Up) || (p2.Action == TouchAction.Up))
                {
                    // End zoom
                    TouchZoomDist = 0;
                    return;
                }
                if (TouchZoomDist > 0)
                {
                    // checking scale
                    var newDist = Distance(p1.Position, p2.Position);
                    var scale = newDist / TouchZoomDist;

                    if (scale > scaleTransform.ScaleX)
                    {
                        if (scaleTransform.ScaleX == 0.7)
                            this.BeginStoryboard((Storyboard)this.Resources["zoomToNotebookStoryboard"]);
                    }
                    else if (scale < scaleTransform.ScaleX)
                    {
                        if (scaleTransform.ScaleX == 1)
                            this.BeginStoryboard((Storyboard)this.Resources["zoomToNotebookToolsStoryboard"]);
                    }
                }
            }
        }    
    } // class
} // namespace
