namespace WpfApplication1
{
    using System;
    using System.Windows.Controls;

    public static class ControlInvokeExtensions
    {
        public static void BeginActionInvoke(this Control control, Action action)
        {
            control.Dispatcher.BeginInvoke(action);
        }
    }
}
