using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ZU.Apps.Austin3
{
    /// <summary>
    /// Represents an entity hosted in the Zoomable Canvas
    /// </summary>
    public partial class Entity
    {
        public double X { get; set; }

        public double Y { get; set; }

        public ImageSource ImageSource { get; set; }

        public string DisplayName { get; set; }

        public Guid Id { get; set; }

        public double Width { get; set; }

        public SolidColorBrush FrontCoverBrush { get; set; }

        public Color FrontCoverColor { get; set; }
    } // class
} // namespace
