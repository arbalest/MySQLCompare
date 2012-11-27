using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySQLCompare.Models;

namespace MySQLCompare.ViewModels
{
    class DbTableComparisonViewModel : ViewModelBase
    {
        private TableComparison _tableComp;
        private string _tableName;
        private List<DbPropertyComparisonViewModel> _properties;
        private bool _isInLeftDb;
        private bool _isInRightDb;

        public DbTableComparisonViewModel(TableComparison tableComp)
            : this(tableComp, true, true)
        {
        }

        public DbTableComparisonViewModel(TableComparison tableComp, bool isInLeftDb, bool isInRightDb)
        {
            _tableComp = tableComp;
            IsInLeftDb = isInLeftDb;
            IsInRightDb = isInRightDb;

            if (_tableComp.LeftTable != null)
            {
                _tableName = _tableComp.LeftTable.Name;
            }
            else if (_tableComp.RightTable != null)
            {
                _tableName = _tableComp.RightTable.Name;
            }
            else
            {
                _tableName = "< Undefined >";
            }

            PopulateProperties();
        }

        public string TableName
        {
            get
            {
                return _tableName;
            }
        }

        public bool IsInLeftDb
        {
            get
            {
                return _isInLeftDb;
            }

            set
            {
                _isInLeftDb = value;
                OnPropertyChanged("IsInLeftDb");
            }
        }

        public bool IsInRightDb
        {
            get
            {
                return _isInRightDb;
            }

            set
            {
                _isInRightDb = value;
                OnPropertyChanged("IsInRightDb");
            }
        }

        public List<DbPropertyComparisonViewModel> Properties
        {
            get 
            {
                if (_properties == null)
                {
                    _properties = new List<DbPropertyComparisonViewModel>();
                }

                return _properties; 
            }
            set { _properties = value; }
        }

        /// <summary>
        /// Uses the TableComparison object to determine which properties belong to each profile in
        /// this view's table, and creates the appropriate viewmodels for each.
        /// </summary>
        public void PopulateProperties()
        {
            if (_tableComp.LeftTable != null)
            {
                foreach (TableProperty prop in _tableComp.LeftTable.Properties)
                {
                    DbPropertyComparisonViewModel propVm = new DbPropertyComparisonViewModel(prop.Name, true, false);
                    Properties.Add(propVm);
                }
            }

            if (_tableComp.RightTable != null)
            {
                foreach (TableProperty prop in _tableComp.RightTable.Properties)
                {
                    DbPropertyComparisonViewModel propVm = new DbPropertyComparisonViewModel(prop.Name, false, true);
                    Properties.Add(propVm);
                }
            }
        }
    }
}
