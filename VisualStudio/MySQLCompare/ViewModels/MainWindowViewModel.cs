using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySQLCompare.Models;
using MySQLCompare.Settings;
using MySQLCompare.Architecture;
using MySQLCompare.EventArguments;

namespace MySQLCompare.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private static string _sqlStatementDivider = "\n\n";

        private ProfilesViewModel _profilesView;
        private DbComparisonViewModel _comparisonView;

        public MainWindowViewModel()
        {
            // Read the settings file to get the available profiles
            List<Profile> profiles = ProfileSettings.getProfiles();
            _profilesView = new ProfilesViewModel(profiles);

            _comparisonView = new DbComparisonViewModel();
            _comparisonView.OnComparisonComplete += new EventHandler<DatabaseComparisonEventArgs>((sender, args) => {
                StatusText = "";
                foreach (TableComparison tableComp in args.Tables)
                {
                    if (tableComp.LeftTable != null && tableComp.RightTable == null && tableComp.LeftTable.CreateStatement != null)
                    {
                        StatusText = StatusText + tableComp.LeftTable.CreateStatement + _sqlStatementDivider;
                    }
                    else if (tableComp.LeftTable != null)
                    {
                        StatusText = StatusText + tableComp.LeftTable.ToString() + _sqlStatementDivider; 
                    }
                }
            });

            CopySqlToClipboardCommand = new AppCommand((sender) => CopySqlToClipboard(sender), (sender) => CanCopySqlToClipboard(sender));
        }

        private string _statusText;
        public string StatusText
        {
            get
            {
                return _statusText;
            }

            set
            {
                _statusText = value;
                OnPropertyChanged("StatusText");
            }
        }

        public ViewModelBase ProfilesView
        {
            get 
            {
                return _profilesView; 
            }
        }

        public ViewModelBase DbComparisonView
        {
            get
            {
                return _comparisonView;
            }
        }

        public AppCommand GenerateSqlCommand
        {
            get;
            set;
        }

        public AppCommand CopySqlToClipboardCommand
        {
            get;
            set;
        }

        public void CopySqlToClipboard(object sender)
        {
            System.Windows.Clipboard.SetData(System.Windows.DataFormats.Text, StatusText);
        }

        public bool CanCopySqlToClipboard(object sender)
        {
            if (StatusText != "")
            {
                return true;
            }

            return false;
        }
    }
}
