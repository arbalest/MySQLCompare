using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySQLCompare.Models;

namespace MySQLCompare.ViewModels
{
    class DbPropertyComparisonViewModel : ViewModelBase
    {
        private TableProperty _property;

        private string _propertyName;
        private string _propertyType;
        private int _propertySize;
        private bool _isInLeftTable;
        private bool _isInRightTable;

        public DbPropertyComparisonViewModel(string propName, bool isInLeftTable, bool isInRightTable)
        {
            PropertyName = propName;
            IsInLeftTable = isInLeftTable;
            IsInRightTable = isInRightTable;
        }

        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

        public string PropertyType
        {
            get;
            set;
        }

        public int PropertySize
        {
            get;
            set;
        }

        public bool IsInLeftTable
        {
            get 
            { 
                return _isInLeftTable; 
            }

            set 
            { 
                _isInLeftTable = value;
                OnPropertyChanged("IsInLeftTable");
            }
        }

        public bool IsInRightTable
        {
            get 
            { 
                return _isInRightTable; 
            }
            
            set 
            { 
                _isInRightTable = value;
                OnPropertyChanged("IsInRightTable");
            }
        }
    }
}
