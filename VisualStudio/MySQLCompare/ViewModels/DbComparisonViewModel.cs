using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading;
using System.Windows.Threading;
using System.Windows;
using System.Collections.ObjectModel;
using MySQLCompare.Models;
using MySQLCompare.Database;
using MySQLCompare.Architecture;
using MySQLCompare.EventArguments;

namespace MySQLCompare.ViewModels
{
    class DbComparisonViewModel : ViewModelBase
    {
        public const string LeftDropTargetElementName = "LeftDropTarget";
        public const string RightDropTargetElementName = "RightDropTarget";

        public const string DefaultLeftDropTargetText = "Drop Left Profile Here";
        public const string DefaultRightDropTargetText = "Drop Right Profile Here";

        private ObservableCollection<DbTableComparisonViewModel> _comparisonTables;

        // Profiles set in the drag and drop profile panels
        private Profile _leftProfile;
        private Profile _rightProfile;

        private bool _isComparingSchemas = false;
        private double _operationProgress;

        public DbComparisonViewModel()
        {
            CompareSchemaCommand = new AppCommand(x => DoCompareSchemaCommand(x), x => CanDoCompareSchemaCommand(x));
            ClearProfilesCommand = new AppCommand(x => DoClearProfilesCommand(x), x => CanDoClearProfilesCommand(x));
        }

        public ObservableCollection<DbTableComparisonViewModel> ComparisonTables
        {
            get 
            {
                if (_comparisonTables == null)
                {
                    _comparisonTables = new ObservableCollection<DbTableComparisonViewModel>();
                }

                return _comparisonTables;
            }

            set
            {
                _comparisonTables = value;
            }
        }

        /// <summary>
        /// Tracks progress of the current running operation.
        /// Useful for displaying progress indicators.
        /// </summary>
        public double OperationProgress
        {
            get
            {
                return _operationProgress;
            }

            set
            {
                _operationProgress = value;
                OnPropertyChanged("OperationProgress");
            }
        }

        public Profile LeftProfile
        {
            get { return _leftProfile; }
            set
            {
                _leftProfile = value;
                OnPropertyChanged("HasLeftProfile");
                OnPropertyChanged("LeftProfileName");
                CompareSchemaCommand.RaiseCanExecuteChanged();
            }
        }

        public string LeftProfileName
        {
            get
            {
                if (LeftProfile != null)
                {
                    return LeftProfile.Name;
                }
                else
                {
                    return DefaultLeftDropTargetText;
                }
            }
        }

        public bool HasLeftProfile
        {
            get
            {
                return (_leftProfile != null) ? true : false;
            }
        }

        public Profile RightProfile
        {
            get { return _rightProfile; }
            set
            {
                _rightProfile = value;
                OnPropertyChanged("HasRightProfile");
                OnPropertyChanged("RightProfileName");
                CompareSchemaCommand.RaiseCanExecuteChanged();
            }
        }

        public string RightProfileName
        {
            get
            {
                if (RightProfile != null)
                {
                    return RightProfile.Name;
                }
                else
                {
                    return DefaultRightDropTargetText;
                }
            }
        }

        public bool HasRightProfile
        {
            get
            {
                return (_rightProfile != null) ? true : false;
            }
        }

        public bool CanCompareProfiles
        {
            get
            {
                if (HasLeftProfile && HasRightProfile)
                {
                    return true;
                }

                return false;
            }
        }

        public AppCommand CompareSchemaCommand
        {
            get;
            set;
        }

        public void DoCompareSchemaCommand(object sender)
        {
            if (LeftProfile != null && RightProfile != null)
            {
                ThreadStart dbCompareThread = delegate()
                {
                    try
                    {
                        List<TableComparison> missingTables = DbComparisons.CompareSchema(LeftProfile, RightProfile, (prog, table) => Application.Current.Dispatcher.BeginInvoke(new Action<double, TableComparison>(UpdateComparisonProgress), DispatcherPriority.Normal, prog, table) );
                        Application.Current.Dispatcher.BeginInvoke(new Action<double, List<TableComparison>>(FinishComparison), DispatcherPriority.Normal, 100, missingTables);
                    }
                    catch (Exception e)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action<Exception>(HandleTableComparisonException), DispatcherPriority.Normal, e);
                    }
                };

                _isComparingSchemas = true;
                OperationProgress = 0;
                ComparisonTables.Clear();

                new Thread(dbCompareThread).Start();
            }
        }

        public bool CanDoCompareSchemaCommand(object sender)
        {
            if (_isComparingSchemas)
            {
                return false;
            }
            else
            {
                return CanCompareProfiles;
            }
        }

        public void HandleTableComparisonException(Exception e)
        {
            MessageBox.Show("There was a problem connecting to the database.\n\n" + e.Message); // TODO: Move to the view
            OperationProgress = 0;
            _isComparingSchemas = false;
        }

        public void UpdateComparisonProgress(double progress, TableComparison newTable)
        {
            // System.Diagnostics.Debug.WriteLine("Reporting Async Progress: " + progress);
            OperationProgress = progress;

            if (newTable != null)
            {
                bool isInLeftDb = (newTable.LeftTable != null);
                bool isInRightDb = (newTable.RightTable != null);

                DbTableComparisonViewModel tableVm = new DbTableComparisonViewModel(newTable, isInLeftDb, isInRightDb);
                ComparisonTables.Add(tableVm);
            }
        }

        public void FinishComparison(double progress, List<TableComparison> tableList)
        {
            OperationProgress = 0;
            _isComparingSchemas = false;
            OnPropertyChanged("CanCompareProfiles");
            OnComparisonComplete(this, new DatabaseComparisonEventArgs(tableList));
        }

        public AppCommand ClearProfilesCommand
        {
            get;
            set;
        }

        public void DoClearProfilesCommand(object sender)
        {
            LeftProfile = null;
            RightProfile = null;
        }

        public bool CanDoClearProfilesCommand(object sender)
        {
            return !_isComparingSchemas;
        }

        public event EventHandler<DatabaseComparisonEventArgs> OnComparisonComplete;
    }
}
