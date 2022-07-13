using OSManager.Model;
using System;

namespace OSManager.View
{
    class MainWindowViewModel : BaseVM
    {
        public MainWindowViewModel()
        {
            if (Properties.Settings.Default.IsAutoUpdate)
                CheckUpdateApp();
        }

        private void CheckUpdateApp()
        {

        }

    }
}
