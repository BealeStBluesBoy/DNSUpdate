using DNSUpdate.Class;
using DNSUpdate.Persistence;

namespace DNSUpdate.Controller
{
    static class SettingsController
    {
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
    }
}
