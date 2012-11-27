using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using MySQLCompare.Models;

namespace MySQLCompare.Database
{
    /// <summary>
    /// Handles all work related to getting information about a database, its tables, or its tables' properties.
    /// </summary>
    class DbTableDescriptions
    {
        public const int SHOWCOL_FIELD = 0;
        public const int SHOWCOL_TYPE = 1;
        public const int SHOWCOL_NULL = 2;
        public const int SHOWCOL_KEY = 3;
        public const int SHOWCOL_DEFAULT = 4;
        public const int SHOWCOL_EXTRA = 5;

        /// <summary>
        /// Lists all tables available in a given connection's database schema.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns>List of tables available.</returns>
        public static List<string> GetTables(DbConnection connection)
        {
            List<string> tables = new List<string>();

            using (IDataReader reader = connection.RunQuery("SHOW TABLES"))
            {
                while (reader.Read())
                {
                    tables.Add(reader.GetString(0));
                }
            }

            return tables;
        }

        /// <summary>
        /// Returns the "CREATE" SQL statement that will create the given table of the given connection's database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="connection"></param>
        /// <returns>The "CREATE" SQL statement.</returns>
        public static string GetCreateTableStatement(string tableName, DbConnection connection)
        {
            string createTableStatement = "";

            using (IDataReader reader = connection.RunQuery("SHOW CREATE TABLE " + tableName))
            {
                while (reader.Read())
                {
                    createTableStatement = reader.GetString(1);
                }
            }

            return createTableStatement;
        }

        /// <summary>
        /// Returns the properties (columns) in the given table of the given connection's database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="connection"></param>
        /// <returns>The list of TableProperty objects describing each individual field.</returns>
        public static List<TableProperty> GetFieldsForTable(string tableName, DbConnection connection)
        {
            List<TableProperty> properties = new List<TableProperty>();
            string previousPropertyName = null;

            using (IDataReader reader = connection.RunQuery("SHOW COLUMNS IN " + tableName))
            {
                while (reader.Read())
                {
                    TableProperty tableProp = new TableProperty(reader.GetValue(SHOWCOL_FIELD).ToString());
                    tableProp.Type = reader.GetValue(SHOWCOL_TYPE).ToString();

                    if (reader.GetValue(SHOWCOL_NULL).ToString() == "YES")
                    {
                        tableProp.IsNullable = true;
                    }
                    else
                    {
                        tableProp.IsNullable = false;
                    }

                    tableProp.Key = reader.GetValue(SHOWCOL_KEY).ToString();

                    if (reader.GetValue(SHOWCOL_DEFAULT) is System.DBNull)
                    {
                        tableProp.Default = null;
                    }
                    else
                    {
                        // System.Diagnostics.Debug.WriteLine("Type of default for " + tableName + "." + reader.GetValue(SHOWCOL_FIELD).ToString() + ":");
                        // System.Diagnostics.Debug.WriteLine(reader.GetValue(SHOWCOL_DEFAULT).GetType());

                        tableProp.Default = reader.GetValue(SHOWCOL_DEFAULT).ToString();
                    }

                    tableProp.Extra = reader.GetValue(SHOWCOL_EXTRA).ToString();

                    if (previousPropertyName != null)
                    {
                        tableProp.AfterProperty = previousPropertyName;
                    }

                    previousPropertyName = tableProp.Name;
                    properties.Add(tableProp);
                }
            }

            return properties;
        }
    }
}
