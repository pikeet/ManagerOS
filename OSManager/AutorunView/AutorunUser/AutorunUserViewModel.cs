using DevExpress.Mvvm;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using OSManager.Model;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using OSManager.StringFormats;
using System.Linq;

namespace OSManager.AutorunView.AutorunUser
{
    class AutorunUserViewModel : BaseVM, IDropTarget
    {
        static readonly RegistryKey openhkcu32 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
        static readonly RegistryKey runhkcu32 = openhkcu32.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
        static readonly RegistryKey runoncehkcu32 = openhkcu32.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce", true);

        static readonly RegistryKey openhkcu64 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
        static readonly RegistryKey runhkcu64 = openhkcu64.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
        static readonly RegistryKey runoncehkcu64 = openhkcu64.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce", true);

        public ObservableCollection<AutorunUserItem> autorunUserItems { get; set; }
        public ICollectionView ItemsView { get; set; }
        public AutorunUserItem SelectedItem { get; set; }
        public AutorunUserViewModel()
        {
            autorunUserItems = new ObservableCollection<AutorunUserItem>();

            if (!Environment.Is64BitOperatingSystem)
            {
                try
                {
                    foreach (String keyName in runhkcu32.GetValueNames())
                    {
                        string keyValue = runhkcu32.GetValue(keyName).ToString();
                        string normalizePath = Normalize.NormalizePath(keyValue);
                        string fileName = Normalize.NormalizePath(normalizePath);
                        autorunUserItems.Add(new AutorunUserItem(normalizePath)
                        {
                            Name = keyName,
                            Tag = 0
                        });
                    }

                    foreach (String keyName in runoncehkcu32.GetValueNames())
                    {
                        string keyValue = runoncehkcu32.GetValue(keyName).ToString();
                        string normalizePath = Normalize.NormalizePath(keyValue);
                        string fileName = Normalize.NormalizePath(normalizePath);
                        autorunUserItems.Add(new AutorunUserItem(normalizePath)
                        {
                            Name = keyName,
                            Tag = 1
                        });
                    }
                }
                catch { }
            }
            else
            {
                try
                {
                    foreach (String keyName in runhkcu64.GetValueNames())
                    {
                        string keyValue = runhkcu64.GetValue(keyName).ToString();
                        string normalizePath = Normalize.NormalizePath(keyValue);
                        string fileName = Normalize.NormalizePath(normalizePath);
                        autorunUserItems.Add(new AutorunUserItem(normalizePath)
                        {
                            Name = keyName,
                            Tag = 2
                        });
                    }

                    foreach (String keyName in runoncehkcu64.GetValueNames())
                    {
                        string keyValue = runoncehkcu64.GetValue(keyName).ToString();
                        string normalizePath = Normalize.NormalizePath(keyValue);
                        string fileName = Normalize.NormalizePath(normalizePath);
                        autorunUserItems.Add(new AutorunUserItem(normalizePath)
                        {
                            Name = keyName,
                            Tag = 3
                        });
                    }
                }
                catch { }
            }

            ItemsView = CollectionViewSource.GetDefaultView(autorunUserItems);
            ItemsView.SortDescriptions.Clear();
            ItemsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }

        public ICommand Remove
        {
            get
            {
                return new DelegateCommand<AutorunUserItem>((aui) =>
                {
                    RemoveItem();
                }, (aui) => SelectedItem != null);
            }
        }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {

            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;

            var dataObject = dropInfo.Data as IDataObject;

            dropInfo.Effects = dataObject.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Copy
                : DragDropEffects.Move;

            MessageBox.Show("DragOver");
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
            dropInfo.Effects = dragFileList.Any(item =>
            {
                foreach (var file in dragFileList)
                {
                    string fileName = Path.GetFileName(file);
                    runhkcu32.SetValue(fileName, file);

                    autorunUserItems.Add(new AutorunUserItem(file)
                    {
                        Name = fileName
                    });
                }
                return dragFileList != null;
            }) ? DragDropEffects.Copy : DragDropEffects.None;

            //var dataObject = dropInfo.Data as DataObject;
            //if (dataObject != null && dataObject.ContainsFileDropList())
            //{
            //    var files = dataObject.GetFileDropList();
            //    foreach (var file in files)
            //    {
            //        string fileName = Path.GetFileName(file);
            //        runhkcu32.SetValue(fileName, file);

            //        autorunUserItems.Add(new AutorunUserItem(file)
            //        {
            //            Name = fileName
            //        });
            //    }
            //}
        }

        private void RemoveItem()
        {
            if(SelectedItem != null)
            {
                if (SelectedItem.Tag == 0)
                {
                    runhkcu32.DeleteValue(SelectedItem.Name);
                }
                else if (SelectedItem.Tag == 1)
                {
                    runoncehkcu32.DeleteValue(SelectedItem.Name);
                }
                else if (SelectedItem.Tag == 2)
                {
                    runhkcu64.DeleteValue(SelectedItem.Name);
                }
                else if (SelectedItem.Tag == 3)
                {
                    runoncehkcu64.DeleteValue(SelectedItem.Name);
                }
                autorunUserItems.Remove(SelectedItem);
            }
        }
    }
}
