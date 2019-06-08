using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ZU.Apps.Austin3
{
    // Platform independence
    public static class PlatformWpf
    {
        private static Dispatcher _dispatcher = null;
        ///<summary>
        /// Initialize WPF platform
        ///</summary>
        public static void Init(UserControl control)
        {
            _dispatcher = control.Dispatcher;
            // Threading abstraction
            Platform.ThreadingCheckAccess = WpfThreadingCheckAccess;
            Platform.ThreadingInvokeInUI = WpfThreadingInvokeInUI;
            Platform.InputCheckInvalidate = WpfInputCheckInvalidate;
        }

        #region Inplemetation
        private static bool WpfThreadingCheckAccess()
        {
            return ((_dispatcher == null) || (_dispatcher.CheckAccess()));
        }

        private static void WpfThreadingInvokeInUI(Action action)
        {
            _dispatcher.BeginInvoke
                (
                    DispatcherPriority.Input,
                    action
                );
        }

        private static void WpfInputCheckInvalidate(string propertyName)
        {
            if (propertyName.StartsWith("Can") | propertyName.StartsWith("Is"))
                CommandManager.InvalidateRequerySuggested();
        }
        #endregion
    }
}
