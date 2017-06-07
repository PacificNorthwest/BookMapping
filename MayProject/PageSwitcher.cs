using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MayProject.Contracts;

namespace MayProject
{
    /// <summary>
    /// Статический класс, управляющий навигацией между страницами
    /// </summary>
    static class PageSwitcher
    {
        public static MainWindow MainWindow { get; set; }

        /// <summary>
        /// Переход на другую страницу приложения
        /// </summary>
        /// <param name="userControl">Страница для перехода</param>
        public static void Switch(UserControl userControl)
        {
            MainWindow.SelectedTab.History.Push(MainWindow.SelectedTab.CurrentPage);
            MainWindow.Navigate(userControl);
        }

        /// <summary>
        /// Перемещение назад по истории страниц
        /// </summary>
        public static void Back()
        {
            MainWindow.Navigate(MainWindow.SelectedTab.History.Pop());
            (MainWindow.SelectedTab.CurrentPage as ISideMenuHandler)?.PopulateSideMenu();
        }
    }
}
