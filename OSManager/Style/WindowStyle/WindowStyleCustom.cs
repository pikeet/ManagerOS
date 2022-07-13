using System.Windows;

namespace OSManager.Style.WindowStyle
{
    public partial class WindowStyleCustom : ResourceDictionary
    {


        private void CloseApplication(object sender, RoutedEventArgs e)
        {
            //var window = (Window)((FrameworkElement)sender).TemplatedParent;
            //window.Close();
            Application.Current.Shutdown();
        }

        private void MinimizeApplication(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.WindowState = WindowState.Minimized;
        }
    }
}
