using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySQLCompare.Models
{
    /// <summary>
    /// Helper object that defines two comparable tables, each of which belongs to a different parent.
    /// Also allows tracking instances where a table appears in one parent set, but not another, by leaving
    /// the respective property null.
    /// </summary>
    class TableComparison
    {
        public TableComparison()
        {
        }

        public TableComparison(Table leftTable)
            : this(leftTable, null)
        {
        }

        public TableComparison(Table leftTable, Table rightTable)
        {
            LeftTable = leftTable;
            RightTable = rightTable;
        }

        public Table LeftTable
        {
            get;
            set;
        }

        public Table RightTable
        {
            get;
            set;
        }
    }
}
