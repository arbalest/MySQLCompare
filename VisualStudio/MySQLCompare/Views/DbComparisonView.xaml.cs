using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySQLCompare.Models;
using MySQLCompare.ViewModels;
using MySQLCompare.Resources;
using MySQLCompare.Utilities;

namespace MySQLCompare.Views
{
    /// <summary>
    /// Interaction logic for DbComparisonView.xaml
    /// </summary>
    public partial class DbComparisonView : UserControl
    {
        public DbComparisonView()
        {
            InitializeComponent();
        }

        public void HandleDragEnter(object sender, DragEventArgs e)
        {
            DbComparisonViewModel viewModel = (DbComparisonViewModel)this.DataContext;

            if (!e.Data.GetDataPresent(AppValues.ProfileDataFormat) || !viewModel.CanDoClearProfilesCommand(this))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        public void HandleDragOver(object sender, DragEventArgs e)
        {
            DbComparisonViewModel viewModel = (DbComparisonViewModel)this.DataContext;

            if (!e.Data.GetDataPresent(AppValues.ProfileDataFormat) || !viewModel.CanDoClearProfilesCommand(this))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        public void HandleDragLeave(object sender, DragEventArgs e)
        {
            DbComparisonViewModel viewModel = (DbComparisonViewModel)this.DataContext;

            if (!e.Data.GetDataPresent(AppValues.ProfileDataFormat) || !viewModel.CanDoClearProfilesCommand(this))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        public void HandleDrop(object sender, DragEventArgs e)
        {
            DbComparisonViewModel viewModel = (DbComparisonViewModel)this.DataContext;

            if (e.Data.GetDataPresent(AppValues.ProfileDataFormat) && viewModel.CanDoClearProfilesCommand(this))
            {
                Border dropTarget = ViewHelpers.FindAncestor<Border>((DependencyObject)e.OriginalSource);

                Profile profile = (Profile)e.Data.GetData(AppValues.ProfileDataFormat);

                if (dropTarget.Name == DbComparisonViewModel.LeftDropTargetElementName)
                {
                    viewModel.LeftProfile = profile;
                }
                else if (dropTarget.Name == DbComparisonViewModel.RightDropTargetElementName)
                {
                    viewModel.RightProfile = profile;
                }
            }
        }
    }
}
