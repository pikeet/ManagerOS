using DevExpress.Mvvm;
using Microsoft.Win32;
using OSManager.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using OSManager.StringFormats;

namespace OSManager.AutorunView.AutorunSystem
{
    class AutorunSystemViewModel : BaseVM
    {
        static readonly RegistryKey openhklm32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
        static readonly RegistryKey runhklm32 = openhklm32.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
        static readonly RegistryKey runoncehklm32 = openhklm32.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce", true);

        static readonly RegistryKey openhklm64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        static readonly RegistryKey runhklm64 = openhklm64.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
        static readonly RegistryKey runoncehklm64 = openhklm64.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce", true);
        public ObservableCollection<AutorunSystemItem> autorunSystemItems { get; set; }
        public ICollectionView ItemsView { get; set; }
        public AutorunSystemItem SelectedItem { get; set; }
        public AutorunSystemViewModel()
        {
            autorunSystemItems = new ObservableCollection<AutorunSystemItem>();

            if (Environment.Is64BitOperatingSystem)
            {
                foreach (String keyName in runhklm64.GetValueNames())
                {
                    string keyValue = runhklm64.GetValue(keyName).ToString();
                    string normalizePath = Normalize.NormalizePath(keyValue);
                    string fileName = Normalize.NormalizePath(normalizePath);
                    autorunSystemItems.Add(new AutorunSystemItem(normalizePath)
                    {
                        Name = keyName,
                        Tag = 0
                    });
                }

                foreach (String keyName in runoncehklm64.GetValueNames())
                {
                    string keyValue = runoncehklm64.GetValue(keyName).ToString();
                    string normalizePath = Normalize.NormalizePath(keyValue);
                    string fileName = Normalize.NormalizePath(normalizePath);
                    autorunSystemItems.Add(new AutorunSystemItem(normalizePath)
                    {
                        Name = keyName,
                        Tag = 1
                    });
                }
            }

            foreach (String keyName in runhklm32.GetValueNames())
            {
                string keyValue = runhklm32.GetValue(keyName).ToString();
                string normalizePath = Normalize.NormalizePath(keyValue);
                string fileName = Normalize.NormalizePath(normalizePath);
                autorunSystemItems.Add(new AutorunSystemItem(normalizePath)
                {
                    Name = keyName,
                    Tag = 2
                });
            }

            foreach (String keyName in runoncehklm32.GetValueNames())
            {
                string keyValue = runoncehklm32.GetValue(keyName).ToString();
                string normalizePath = Normalize.NormalizePath(keyValue);
                string fileName = Normalize.NormalizePath(normalizePath);
                autorunSystemItems.Add(new AutorunSystemItem(normalizePath)
                {
                    Name = keyName,
                    Tag = 3
                });
            }


            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);
                foreach (String file in Directory.GetFiles(path))
                {
                    string normalizePath = Normalize.NormalizePath(file);
                    if (file.Length > 0)
                    {
                        string extension = Path.GetExtension(file);
                        if (extension != null && (extension.Equals(".lnk")))
                        {
                            string FileName = Path.GetFileName(file);
                            autorunSystemItems.Add(new AutorunSystemItem(normalizePath)
                            {
                                Name = FileName,
                                Tag = 4
                            });
                        }
                    }
                }

            ItemsView = CollectionViewSource.GetDefaultView(autorunSystemItems);
            ItemsView.SortDescriptions.Clear();
            ItemsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }

        public ICommand Remove
        {
            get
            {
                return new DelegateCommand<AutorunSystemItem>((stu) =>
                {
                    RemoveItem();
                }, (stu) => SelectedItem != null);
            }
        }

        private void RemoveItem()
        {
            if (SelectedItem.Tag == 0)
            {
                runhklm64.DeleteValue(SelectedItem.Name);
            }
            else if (SelectedItem.Tag == 1)
            {
                runoncehklm64.DeleteValue(SelectedItem.Name);
            }
            else if (SelectedItem.Tag == 2)
            {
                runhklm32.DeleteValue(SelectedItem.Name);
            }
            else if (SelectedItem.Tag == 3)
            {
                runoncehklm32.DeleteValue(SelectedItem.Name);
            }
            else if (SelectedItem.Tag == 4)
            {
                File.Delete(SelectedItem.FullPath);
            }
            autorunSystemItems.Remove(SelectedItem);
        }

    }
}
