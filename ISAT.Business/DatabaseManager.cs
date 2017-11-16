using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using Csla.Data;

namespace ISAT.Business
{
    public class Database
    {
        public static string ConnectionString;
        public static MySqlConnection _connection;
        public static MySqlTransaction _transaction;
        public static bool DbTransaction;
        public static SafeDataReader DataReader;
        public static void SetConnectionString(string connectionName)
        {
            Database.ConnectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }
    }

    public class DatabaseManager : IDisposable
    {
        public DatabaseManager()
        {
            if (Database._connection == null)
            {
                Database._connection = GetConnection();
            }
        }

        public DatabaseManager(bool usingTransaction)
        {
            if (usingTransaction)
            {
                Database._connection = GetConnection(usingTransaction);
            }
            else
            {
                Database._connection = GetConnection();
            }
        }

        private MySqlConnection GetConnection()
        {
            if (Database._connection == null)
            {
                Database._connection = new MySqlConnection(Database.ConnectionString);
            }
            if (Database._connection.State != System.Data.ConnectionState.Open)
            {
                Database._connection.Open();
            }
            return Database._connection;
        }

        private MySqlConnection GetConnection(bool usingTransaction)
        {
            if (Database._connection == null)
            {
                Database._connection = new MySqlConnection(Database.ConnectionString);
            }
            if (Database._connection.State != System.Data.ConnectionState.Open)
            {
                Database._connection.Open();
            }

            if (usingTransaction && Database._transaction==null)
            {
                Database._transaction = Database._connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                Database.DbTransaction = true;
            }

            return Database._connection;
        }

        public MySqlCommand CreateCommand()
        {
            if (Database.DataReader != null && !Database.DataReader.IsClosed)
            {
                Database.DataReader.Close();
            }
            MySqlCommand cm = GetConnection().CreateCommand();
            cm.CommandType = System.Data.CommandType.Text;
            if (Database.DbTransaction)
            {
                cm.Transaction = Database._transaction;
            }
            return cm;
        }

        public MySqlCommand CreateSelectCommand()
        {
            MySqlCommand cm = GetConnection().CreateCommand();
            cm.CommandType = System.Data.CommandType.Text;
            return cm;
        }

        public SafeDataReader Read(MySqlCommand cm)
        {
            if (Database.DataReader == null )
            {
                Database.DataReader = new SafeDataReader(cm.ExecuteReader());
            }
            else
            {
                if (!Database.DataReader.IsClosed)
                {
                    Database.DataReader.Close();
                    Database.DataReader = null;
                    Read(cm);
                }
                else
                {
                    Database.DataReader = new SafeDataReader(cm.ExecuteReader());
                }
            }

            return Database.DataReader;
        }

        public void SaveChanges()
        {
            if (Database._transaction != null)
            {
                try
                {
                    Database._transaction.Commit();
                    Database._transaction = null;
                }
                catch (Exception ex)
                {
                    Database._transaction.Rollback();
                    Console.WriteLine(ex.Message);
                }

            }
            
        }

        public void Dispose()
        {
            if (Database._transaction == null)
            {
                if (Database.DataReader !=null && !Database.DataReader.IsClosed)
                {
                    Database.DataReader.Close();
                }
                Database._connection.Close();
                
                Database._transaction = null;
                Database._connection = null;
            }
            if (Database.DataReader != null && !Database.DataReader.IsClosed)
            {
                Database.DataReader.Close();
            }
        }
    }
}
