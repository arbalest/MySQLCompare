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
using MySQLCompare.ViewModels;
using MySQLCompare.Models;
using MySQLCompare.Resources;
using MySQLCompare.Utilities;

namespace MySQLCompare.Views
{
    /// <summary>
    /// Interaction logic for ProfilesView.xaml
    /// </summary>
    public partial class ProfilesView : UserControl
    {
        private Point startingClick;

        public ProfilesView()
        {
            InitializeComponent();
        }

        #region Mouse Event Handlers

        /**
         * Mouse Event Handlers
         * TODO:
         * - Add AppCommands to handle this functionality in the ViewModels
         */

        public void HandleMouseClick(object sender, MouseButtonEventArgs e)
        {
            startingClick = e.GetPosition(null);
        }

        public void HandleMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePosition = e.GetPosition(null);
            Vector movement = startingClick - mousePosition;
            bool beginDrag = false;

            beginDrag = (Math.Abs(movement.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(movement.Y) > SystemParameters.MinimumVerticalDragDistance);

            if (e.LeftButton == MouseButtonState.Pressed && beginDrag == true)
            {
                ListBoxItem item = ViewHelpers.FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);

                if (item != null)
                {
                    ProfileItemViewModel profileModel = (ProfileItemViewModel)item.DataContext;
                    Profile profile = profileModel.Profile;
                    DataObject draggedProfile = new DataObject(AppValues.ProfileDataFormat, profile);
                    DragDrop.DoDragDrop(item, draggedProfile, DragDropEffects.Copy);
                }
            }
        }

        public void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = false;
        }

        #endregion
    }
}
