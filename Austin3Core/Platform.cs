using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZU.Apps.Austin3
{
    // Platform independence
    public static class Platform
    {
        #region Threading and input abstraction
        public static Func<bool> ThreadingCheckAccess = null;
        /*/
        WPF:
            return ( (Application.Current==null) || (Application.Current.Dispatcher.CheckAccess()) );
        /*/
        public static Action<Action> ThreadingInvokeInUI = null;
        /*/
        WPF:
                    Application.Current.Dispatcher.Invoke
                        (
                            System.Windows.Threading.DispatcherPriority.Input, // Normal
                            action
                        );
        /*/
        public static Action<string> InputCheckInvalidate = null;
        /*/
        WPF:
                if (propertyName.StartsWith("Can")|propertyName.StartsWith("Is"))
                    CommandManager.InvalidateRequerySuggested();
        /*/
        public static void InvokeInUI(Action action)
        {
            if ((ThreadingCheckAccess == null) || (ThreadingInvokeInUI == null) || (ThreadingCheckAccess()))
            {
                action();
            }
            else
            {
                ThreadingInvokeInUI(action);
            }
        }

        public static void InputInvalidate(string propertyName)
        {
            if (InputCheckInvalidate != null)
                InputCheckInvalidate(propertyName);
        }
        #endregion
    }
}
