using OSManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSManager.SettingsView.GeneralSettings
{
    class GeneralSettingsViewModel : BaseVM
    {
        public bool _autoUpdate;
        public bool AutoUpdate
        {
            get { return _autoUpdate; }
            set
            {
                _autoUpdate = value;
                RaisePropertyChanged("AutoUpdate");

                Properties.Settings.Default.IsAutoUpdate = value;
                Properties.Settings.Default.Save();
            }
        }
        public GeneralSettingsViewModel()
        {
            AutoUpdate = Properties.Settings.Default.IsAutoUpdate;
        }
    }
}
