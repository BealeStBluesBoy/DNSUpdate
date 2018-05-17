using DNSUpdate.Class;
using System;
using System.Data.SQLite;

namespace DNSUpdate.Persistence
{
    class SettingsPersistence : Persistence
    {
        public SettingsPersistence()
        {
            
        }

        public bool Create()
        {
            if (OpenConnection())
            {
                string Query = "CREATE TABLE IF NOT EXISTS Settings (Domain TEXT, Token TEXT, Interval INTEGER);";
                SQLiteCommand Command = new SQLiteCommand(Query, Connection);
                Command.ExecuteNonQuery();
                SQLiteDataReader Reader = Command.ExecuteReader();
                if (!Reader.HasRows)
                {
                    Query = "INSERT INTO Settings (Domain, Token, Interval) VALUES ('', '', 0)";
                    Command = new SQLiteCommand(Query, Connection);
                    Command.ExecuteNonQuery();
                }
                CloseConnection();
                return true;
            }
            return false;
        }

        public Settings Select()
        {
            Settings ret = new Settings("", "", 0);
            if (OpenConnection())
            {
                string Query = "SELECT * FROM Settings;";
                SQLiteCommand Command = new SQLiteCommand(Query, Connection);
                SQLiteDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows && Reader.Read())
                {
                    ret = new Settings(Reader["Domain"].ToString(), Reader["Token"].ToString(), byte.Parse(Reader["Interval"].ToString()));
                    Reader.Close();
                }
                CloseConnection();
            }
            return ret;
        }

        public bool Update(string domain, string token, byte interval)
        {
            if (OpenConnection())
            {
                string Query = String.Format("UPDATE Settings SET Domain = '{0}', Token = '{1}', Interval = '{2}';", domain, token, interval);
                SQLiteCommand Command = new SQLiteCommand(Query, Connection);
                Command.ExecuteNonQuery();
                CloseConnection();
                return true;
            }
            return false;
        }
    }
}
