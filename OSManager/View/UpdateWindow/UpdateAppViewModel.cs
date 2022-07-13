using OSManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OSManager.View.UpdateWindow
{
    class UpdateAppViewModel : BaseVM
    {

        public double left { get; set; }
        public double top { get; set; }

        public UpdateAppViewModel()
        {
            double _width = 440;
            double _height = 330;
            var primaryMonitorArea = SystemParameters.WorkArea;
            left = primaryMonitorArea.Right - _width - 10;
            top = primaryMonitorArea.Bottom - _height - 10;
        }
    }
}
