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
using MayProject.Controller;
using System.Windows.Markup;

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для RelationsMapPage.xaml
    /// </summary>
    public partial class RelationsMapPage : UserControl
    {
        private Book _book;

        public RelationsMapPage(Book book)
        {
            _book = book;
            InitializeComponent();
        }       
    }
}
