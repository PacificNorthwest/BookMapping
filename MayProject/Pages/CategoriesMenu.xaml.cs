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

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для CategoriesMenu.xaml
    /// </summary>
    public partial class CategoriesMenu : UserControl
    {
        private Book _book;

        public CategoriesMenu(Book book)
        {
            this._book = book;
            InitializeComponent();
        }
    }
}
