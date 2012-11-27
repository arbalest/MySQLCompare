using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySQLCompare.Models;

namespace MySQLCompare.EventArguments
{
    class DatabaseComparisonEventArgs : EventArgs
    {
        public DatabaseComparisonEventArgs(List<TableComparison> tableList)
        {
            Tables = tableList;
        }

        public List<TableComparison> Tables
        {
            get;
            set;
        }
    }
}
