using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace System.Windows.Controls
{
    partial class ZoomableCanvas
    {
        #region IsBusyProperty
        /// <summary>
        /// Identifies the <see cref="IsBusy"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register
            (
                "IsBusy",
                typeof(bool),
                typeof(ZoomableCanvas),
                new FrameworkPropertyMetadata(true)
            );

        /// <summary>
        /// Gets or sets whether control is busy or not.
        /// </summary>
        /// <value><c>true</c> or <c>false</c>.  The default is <see cref="true"/>.</value>
        public bool IsBusy
        {
            get
            {
                return (bool)GetValue(IsBusyProperty);
            }
            set
            {
                SetValue(IsBusyProperty, value);
            }
        }
        #endregion IsBusyProperty
    }
}//namespace
