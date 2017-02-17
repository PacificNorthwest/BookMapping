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
            mainWindow.Navigate(userControl);
        }
    }
}
