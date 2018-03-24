using System;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace DNSUpdate
{
    abstract class Persistence
    {
        protected SQLiteConnection Connection;

        public Persistence()
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DNSUpdate");
            Connection = new SQLiteConnection("Data Source = " + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DNSUpdate\\Settings.db") + "; Version = 3;");
        }

        protected bool OpenConnection()
        {
            try
            {
                Connection.Open();
                return true;
            }
            catch (SQLiteException ex)
            {
                switch (ex.ErrorCode)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to database.  Contact administrator");
                        break;
                }
                return false;
            }
        }

        protected bool CloseConnection()
        {
            try
            {
                Connection.Close();
                return true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
