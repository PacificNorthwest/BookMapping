using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MayProject.Contracts;

namespace MayProject
{
    static class PageSwitcher
    {
        public static MainWindow mainWindow { get; set; }

        public static void Switch(UserControl userControl)
        {
            MainWindow.SelectedTab.History.Push(MainWindow.SelectedTab.CurrentPage);
            mainWindow.Navigate(userControl);
        }

        public static void Back()
        {
            mainWindow.Navigate(MainWindow.SelectedTab.History.Pop());
            (MainWindow.SelectedTab.CurrentPage as ISideMenuHandler)?.PopulateSideMenu();
        }
    }
}
