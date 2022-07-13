using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSManager.WinTool
{
    public class WinInfo
    {
        public static string GetOsVersion()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                using (var reg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = reg.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\"))
                {
                    return key.GetValue("ProductName").ToString();
                }
            }
            else
            {
                using (var reg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                using (var key = reg.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\"))
                {
                    return key.GetValue("ProductName").ToString();
                }
            }
        }
    }
}
