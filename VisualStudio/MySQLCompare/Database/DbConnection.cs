using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace MySQLCompare.Database
{
    /// <summary>
    /// Convenience wrapper around the MySQL ADO.Net classes to reduce the code needed for running simple
    /// queries with results.
    /// </summary>
    class DbConnection : IDisposable
    {
        protected const string connectionStringFormat = "server={0};port={1};uid={2};pwd={3};database={4}";
        protected string connectionString;
        private bool _disposed;
        protected MySqlConnection connection;
        protected MySqlCommand command;

        public DbConnection(string host, int port, string username, string password, string schema)
        {
            connectionString = string.Format(connectionStringFormat, host, port, username, password, schema);

            command = new MySqlCommand();
        }

        public bool Open()
        {
            bool connectionSuccessful = false;

            // Make sure the connection is not already open
            if (connection != null)
            {
                return connectionSuccessful;
            }

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                command.Connection = connection;
                connectionSuccessful = true;
            }
            catch (Exception e)
            {
                // TODO: Handle connection errors more usefully.
                System.Diagnostics.Debug.WriteLine(e);
            }

            return connectionSuccessful;
        }

        public MySqlDataReader RunQuery(string queryString)
        {
            if (command.Connection != null)
            {
                command.CommandText = queryString;
                MySqlDataReader reader = command.ExecuteReader();
                return reader;
            }
            else
            {
                throw new ApplicationException("No database connection found.");
            }
        }

        public void Close()
        {
            if (connection != null)
            {
                connection.Close();
                command.Connection = null;
                connection = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    command.Connection = null;
                    command.Dispose();
                }

                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }

                this._disposed = true;
            }
        }

        ~DbConnection()
        {
            Dispose(false);
        }
    }
}
