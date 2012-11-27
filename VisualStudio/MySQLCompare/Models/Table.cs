using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySQLCompare.Models
{
    class Table
    {
        public Table(string tableName)
            : this(tableName, new List<TableProperty>())
        {
        }

        public Table(string tableName, List<TableProperty> tableProperties)
        {
            Name = tableName;
            Properties = tableProperties;
        }

        public string Name
        {
            get;
            set;
        }

        public string CreateStatement
        {
            get;
            set;
        }

        public List<TableProperty> Properties
        {
            get;
            set;
        }

        /// <summary>
        /// Generates the "ALTER TABLE ... ADD COLUMN ..." statement that would add the fields found in Properties to a database table.
        /// </summary>
        /// <returns>The "ALTER TABLE ... ADD COLUMN ..." SQL statement</returns>
        public override string ToString()
        {
            StringBuilder generatedSql = new StringBuilder();

            if (Properties != null)
            {
                int propCount = Properties.Count;
                if (propCount > 0)
                {
                    generatedSql.Append("ALTER TABLE " + Name);
                    int currentProp = 1;

                    foreach (TableProperty prop in Properties)
                    {
                        generatedSql.Append(" ADD COLUMN ");
                        generatedSql.Append(prop.ToString());

                        if (prop.AfterProperty != null)
                        {
                            generatedSql.Append(" AFTER ");
                            generatedSql.Append(prop.AfterProperty);
                        }

                        if (currentProp == propCount)
                        {
                            generatedSql.Append(";");
                        }
                        else
                        {
                            generatedSql.Append(",\n");
                        }

                        currentProp++;
                    }
                }
            }

            return generatedSql.ToString();
        }
    }
}
