using OSManager.Model;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OSManager.AutorunView.AutorunSystem
{
    class AutorunSystemItem : BaseVM
    {
        public string Name { get; set; }
        public ImageSource Icon { get; }
        public string FullPath { get; }
        public int Tag { get; set; }

        public AutorunSystemItem(string path)
        {
            try
            {
                FullPath = path;
                if (!string.IsNullOrEmpty(path))
                {
                    using (var icon = System.Drawing.Icon.ExtractAssociatedIcon(path))
                    {
                        Icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                                    icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    }
                }
                else
                {
                    using (var icon = Properties.Resources.Blank)
                    {
                        Icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                                    icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    }
                }
            }
            catch { }
        }
    }
}
