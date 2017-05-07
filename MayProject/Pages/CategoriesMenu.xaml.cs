using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MayProject.DataModel;
using MayProject.Contracts;

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для CategoriesMenu.xaml
    /// </summary>
    public partial class CategoriesMenu : UserControl
    {
        private Book _book;
        public string BookTitle => _book.Title;

        public CategoriesMenu(Book book)
        {
            this._book = book;
            InitializeComponent();
        }

        private void CategoryClick(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).DataContext as string)
            {
                case "Notes":
                    {
                        PageSwitcher.Switch(new ElementMenu(_book.Notes));
                        break;
                    }
                case "Characters":
                    {
                        PageSwitcher.Switch(new ElementMenu(_book.Characters));
                        break;
                    }
                case "Chapters":
                    {
                        PageSwitcher.Switch(new ElementMenu(_book.Chapters));
                        break;
                    }
                case "Locations":
                    {
                        PageSwitcher.Switch(new ElementMenu(_book.Locations));
                        break;
                    }
            }
        }
    }
}
