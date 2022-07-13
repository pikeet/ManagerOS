using Microsoft.Win32;
using OSManager.Model;
using OSManager.WinTool;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Management;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OSManager.MainView
{
    class MainControlViewModel : BaseVM
    {
        public BitmapImage WinLogo { get; set; }
        public string Processor { get; set; }
        public string Motherboard { get; set; }
        public string Video { get; set; }
        public string OZY { get; set; }
        public string VersionsOS { get; set; }
        public string Versions { get; set; }

        public MainControlViewModel()
        {
            Processor = GetHardwareInfo("Win32_Processor", "Name");
            Motherboard = GetHardwareInfo("Win32_BaseBoard", "Manufacturer") + " " + GetHardwareInfo("Win32_BaseBoard", "Product");
            Video = GetHardwareInfo("Win32_VideoController", "Name");

            VersionsOS = "Версия Windows •" + WinInfo.GetOsVersion().Replace("Windows", "");
            Versions = "Версия ПО • " + Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", "");

            long originalOperativ = long.Parse(GetHardwareInfo("Win32_ComputerSystem", "TotalPhysicalMemory"));
            OZY = FormatBytes(originalOperativ);
        }

        private static string FormatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "МБ", "ГБ", "ТБ" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1000;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        private static string GetHardwareInfo(string WIN32_Class, string ClassItemField)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM " + WIN32_Class);
            try
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    return obj[ClassItemField].ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
