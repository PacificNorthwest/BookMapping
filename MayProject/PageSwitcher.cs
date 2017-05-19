using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MayProject
{
    static class PageSwitcher
    {
        public static MainWindow mainWindow { get; set; }

        public static void Switch(UserControl userControl)
        {
            ((MainWindow.CurrentItem.DataContext as Dictionary<string, object>)["History"] as Stack<UserControl>)
                .Push(((MainWindow.CurrentItem
                .Content as Grid)
                .Children[1] as Grid)
                .Children[0] as UserControl);
            //MainWindow.History.Push(((MainWindow.CurrentItem.Content as Grid).Children[1] as Grid).Children[0] as UserControl);
            mainWindow.Navigate(userControl);
        }

        public static void Back()
        {
            if (((MainWindow.CurrentItem.DataContext as Dictionary<string, object>)["History"] as Stack<UserControl>).Count > 0)
                mainWindow.Navigate(((MainWindow.CurrentItem.DataContext as Dictionary<string, object>)["History"] as Stack<UserControl>).Pop());
        }
    }
}
