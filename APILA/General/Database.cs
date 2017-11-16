using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace APILA.General
{
    public class Database : IDisposable
    {
        public static string ConnectionString;
        private static MySqlConnection _connection;
        private static MySqlTransaction _transaction;
        private static bool DbTransaction;

        public Database()
        {
            if (_connection==null)
            {
                _connection = GetConnection();
            }
        }

        public Database(bool usingTransaction)
        {
            if (usingTransaction)
            {
                _connection = GetConnection(usingTransaction);
            }
            else
            {
                _connection = GetConnection();
            }
        }

        private  MySqlConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new MySqlConnection(ConnectionString);
            }
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
            return _connection;
        }

        private MySqlConnection GetConnection(bool usingTransaction)
        {
            if (_connection == null)
            {
                _connection = new MySqlConnection(ConnectionString);
            }
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }

            if (usingTransaction)
            {
                _transaction = _connection.BeginTransaction();
                DbTransaction = true;
            }

            return _connection;
        }

        public MySqlCommand CreateCommand()
        {
            MySqlCommand cm = GetConnection().CreateCommand();
            if (DbTransaction)
            {
                cm.Transaction = _transaction;
            }
            return cm;
        }

        public MySqlCommand CreateSelectCommand()
        { 
            return GetConnection().CreateCommand();
        }

        public static void SetConnectionString(string connectionName)
        {
            ConnectionString= ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public void Dispose()
        {
            if (!DbTransaction)
            {
                if (_transaction != null)
                {
                    _transaction.Commit();
                }

                _connection.Close();

                _transaction = null;
                _connection = null;
            }
            
        }
    }
}
