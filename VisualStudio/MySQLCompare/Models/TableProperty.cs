using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySQLCompare.Models
{
    class TableProperty
    {
        public TableProperty(string propertyName)
        {
            Name = propertyName;
        }

        public string Name
        {
            get;
            set;
        }

        public string AfterProperty
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public bool IsNullable
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }

        public string Default
        {
            get;
            set;
        }

        public string Extra
        {
            get;
            set;
        }

        /// <summary>
        /// Generates the SQL statement that describes this property, and could be used in an "ALTER TABLE" statement
        /// to add or modify this property in a single database table.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();

            buffer.Append(Name + " " + Type);

            if (IsNullable)
            {
                buffer.Append(" NULL");
            }
            else
            {
                buffer.Append(" NOT NULL");
            }

            if (Default != null)
            {
                if (Type.StartsWith("varchar", StringComparison.InvariantCultureIgnoreCase) || Type.StartsWith("char", StringComparison.InvariantCultureIgnoreCase) || Type.StartsWith("text", StringComparison.InvariantCultureIgnoreCase))
                {
                    buffer.Append(" DEFAULT '" + Default + "'");
                }
                else
                {
                    buffer.Append(" DEFAULT " + Default);
                }
            }
            else
            {
                if (IsNullable)
                {
                    buffer.Append(" DEFAULT NULL");
                }
            }

            if (Extra.Contains("auto_increment"))
            {
                buffer.Append(" AUTO_INCREMENT");
            }

            if (Key == "PRI")
            {
                buffer.Append(" PRIMARY KEY");
            }
            else if (Key == "UNI")
            {
                buffer.Append(" UNIQUE KEY");
            }

            return buffer.ToString();
        }
    }
}
