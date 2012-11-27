using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using MySQLCompare.ViewModels;

/**
 * Application Notes:
 * -------------------------------------------------
 * 
 * Feature Summary
 *   - Tables are compared "unidirectionally" only, where tables in the left database that are missing from the right are shown (but not vice-versa).
 *   - Properties are compared "multidirectionally" for a given table that's been found in both the left and right databases, however, the SQL generated
 *      currently only supports the statements that would bring the left database to parity with the right (but not vice-versa).
 */

namespace MySQLCompare
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow mainWindow = new MainWindow();
            MainWindowViewModel viewModel = new MainWindowViewModel();

            mainWindow.DataContext = viewModel;
            mainWindow.Show();
        }
    }
}
