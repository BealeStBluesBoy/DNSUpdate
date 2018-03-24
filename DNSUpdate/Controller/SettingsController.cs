namespace DNSUpdate
{
    class SettingsController
    {
        public bool CreateSettings()
        {
            SettingsPersistence db = new SettingsPersistence();
            return db.Create();
        }

        public Settings GetSettings()
        {
            SettingsPersistence db = new SettingsPersistence();
            return db.Select();
        }

        public bool ResetSettings()
        {
            SettingsPersistence db = new SettingsPersistence();
            return db.Update("", "", 0);
        }

        public bool SetSettings(string domain, string token, byte interval)
        {
            SettingsPersistence db = new SettingsPersistence();
            return db.Update(domain, token, interval);
        }
    }
}
