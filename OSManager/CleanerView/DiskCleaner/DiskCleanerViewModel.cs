using DevExpress.Mvvm;
using OSManager.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace OSManager.CleanerView.DiskCleaner
{
    class DiskCleanerViewModel : BaseVM
    {
        private DriveInforfmations _selectedItem;
        private bool _cleanNullFolder;

        public ObservableCollection<DriveInforfmations> driveInforfmations { get; set; }
        public ICollectionView ItemsView { get; set; }
        public DriveInforfmations SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                SelectedDiskName = $"Выбран диск - {SelectedItem.Drive}";
            }
        }

        public string SelectedDiskName { get; set; }
        public bool cleanDuplicateFile { get; set; }
        public bool cleanNullFolder
        {
            get { return _cleanNullFolder; }
            set
            {
                _cleanNullFolder = value;
                cbIsChecked = cleanNullFolder;
            }
        }
        public bool cbIsChecked { get; set; }


        public DiskCleanerViewModel()
        {
            SelectedDiskName = "Выберите диск для очистки";
            cbIsChecked = false;

            driveInforfmations = new ObservableCollection<DriveInforfmations>();
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType == DriveType.Fixed)
                {
                    if (Path.GetPathRoot(Environment.SystemDirectory) == d.Name)
                        driveInforfmations.Add(new DriveInforfmations(d.Name, d.TotalSize, d.TotalFreeSpace, d, Properties.Resources.sdisk));
                    else
                        driveInforfmations.Add(new DriveInforfmations(d.Name, d.TotalSize, d.TotalFreeSpace, d, Properties.Resources.disk));
                }
            }
            ItemsView = CollectionViewSource.GetDefaultView(driveInforfmations);

        }

        public ICommand startClean
        {
            get
            {
                return new DelegateCommand<DriveInforfmations>((di) =>
                {
                    if (cleanNullFolder)
                        RemoveNullDirectoryAsync(SelectedItem.Drive);
                }, (di) => SelectedItem != null);
            }
        }

        private async void RemoveNullDirectoryAsync(string startLocation)
        {
            await Task.Run(() => RemoveNullDirectory(startLocation));
        }

        private void RemoveNullDirectory(string startLocation)
        {
            foreach (var directory in Directory.GetDirectories(startLocation))
            {
                try
                {
                    RemoveNullDirectory(directory);
                    if (Directory.GetFiles(directory).Length == 0 &&
                        Directory.GetDirectories(directory).Length == 0)
                    {
                        Directory.Delete(directory, false);
                        using (StreamWriter sw = new StreamWriter(@"log.txt", true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine(directory);
                        }
                    }
                }
                catch { }
            }
        }
    }
}
