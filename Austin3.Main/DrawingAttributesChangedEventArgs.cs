using System;
using System.Windows.Ink;

namespace ZU.Apps.Austin3
{
    public class DrawingAttributesChangedEventArgs : EventArgs
    {
        public DrawingAttributes DrawingAttributes { get; set; }
        
        public DrawingAttributesChangedEventArgs(DrawingAttributes attributes)
        {
            this.DrawingAttributes = attributes;
        }
    } // class
} // namespace