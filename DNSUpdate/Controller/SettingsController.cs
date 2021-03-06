﻿using DNSUpdate.Class;
using DNSUpdate.Persistence;
using Microsoft.Win32;

namespace DNSUpdate.Controller
{
    static class SettingsController
    {
        private static RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public static bool CreateSettings()
        {
            SettingsPersistence db = new SettingsPersistence();
            return db.Create();
        }

        public static Settings GetSettings()
        {
            SettingsPersistence db = new SettingsPersistence();
            return db.Select();
        }

        public static bool ResetSettings()
        {
            SettingsPersistence db = new SettingsPersistence();
            return db.Update("", "", 0);
        }

        public static bool SetSettings(string domain, string token, byte interval)
        {
            SettingsPersistence db = new SettingsPersistence();
            return db.Update(domain, token, interval);
        }

        public static void SetOnStartup()
        {
            rk.SetValue("DNSUpdate", System.IO.Path.GetFullPath(System.Reflection.Assembly.GetExecutingAssembly().Location));
        }

        public static void UnsetOnStartup()
        {
            rk.DeleteValue("DNSUpdate", false);
        }

        public static bool IsSettedOnStartup()
        {
            return rk.GetValue("DNSUpdate") != null;
        }
    }
}
