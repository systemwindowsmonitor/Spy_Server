using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;

namespace BrowserHistory_Server.Data
{
    class DbManager
    {
        string dataBaseName;
        SQLiteConnection connection;
        public DbManager(string dataBaseName)
        {
            this.dataBaseName = dataBaseName;
        }

        #region  подключение/отключение БД
        public void Connect()
        {
            try
            {
                connection = new SQLiteConnection(string.Format($"Data Source={dataBaseName};"));
                connection.Open();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public void Disconnect()
        {
            try
            {
                if (this.connection != null)
                    if (this.connection.State == System.Data.ConnectionState.Open)
                    {
                        this.connection.ClearCachedSettings();
                        this.connection.ClearTypeCallbacks();
                        this.connection.ClearTypeMappings();
                        this.connection.Close();
                        this.connection.Dispose();
                        this.connection = null;
                    }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region взаимодействие с логином/паролем
        public bool  CheckLogin(string login)
        {
            return GetLogins(login) > 0 ? true : false;
        }
        public bool CheckPassword(string crypt_password)
        {
            return GetPasswords(crypt_password) > 0 ? true : false; ;
        }
        #endregion

        #region методы выборки с таблиц
        public List<string> GetLogins() {
            List<string> data = new List<string>();
            SQLiteCommand command = new SQLiteCommand("SELECT login FROM AuthorizationData;", connection);
            using (SQLiteDataReader reader = command.ExecuteReader()){
                foreach (DbDataRecord record in reader)
                {
                    data.Add(record["login"].ToString());
                }
            }
            return data;
        }
        private int GetLogins(string key) {
            try
            {
                SQLiteCommand command = new SQLiteCommand("SELECT id FROM AuthorizationData where login = @login;", connection);
                command.Parameters.Add(new SQLiteParameter("@login", key));
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    return reader.FieldCount;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
          
            return 0;
        }
        public List<string> GetPasswords() {
            List<string> data = new List<string>();
            SQLiteCommand command = new SQLiteCommand("SELECT password FROM AuthorizationData;", connection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                foreach (DbDataRecord record in reader)
                {
                    data.Add(record["password"].ToString());
                }
            }
            return data;
        }
        private int GetPasswords(string key)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand("SELECT id FROM AuthorizationData where password = @password;", connection);
                command.Parameters.Add(new SQLiteParameter("@password", key));
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    return reader.FieldCount;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

            return 0;
        }
        #endregion
    }
}
