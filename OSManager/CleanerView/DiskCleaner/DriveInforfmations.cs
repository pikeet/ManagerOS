using OSManager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OSManager.CleanerView.DiskCleaner
{
    class DriveInforfmations : BaseVM
    {
        public string Drive { get; set; }
        public long DriveCapacity { get; set; }
        public long DriveFreeSpace { get; set; }
        public DriveInfo Tag { get; set; }
        public ImageSource Icon { get; set; }

        public DriveInforfmations(string driveName, long driveCapacity, long driveFreeSpace, DriveInfo di, System.Drawing.Icon icon)
        {
            Drive = driveName;
            DriveCapacity = driveCapacity;
            DriveFreeSpace = driveFreeSpace;
            Tag = di;
            Icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
