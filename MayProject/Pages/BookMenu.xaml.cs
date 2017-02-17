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
using System.Windows.Markup;
using MayProject;
using MayProject.Controller;
using MayProject.DataModel;

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для BookMenu.xaml
    /// </summary>
    public partial class BookMenu : UserControl
    {
        public BookMenu()
        {
            InitializeComponent();
            Bookshelf.Books.Load();
            Visualize();
        }

        private void Visualize()
        {
            foreach (Book book in Bookshelf.Books)
            {
                StringBuilder buttonXaml = new StringBuilder();
                buttonXaml.Append(@"<Button xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' 
                                     xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' ");
                buttonXaml.Append($"Content = '{book.Title}' Margin = '20' FontSize = '30'");
                buttonXaml.Append("/>");
                Button button = XamlReader.Parse(buttonXaml.ToString()) as Button;
                button.Click += Button_Click;
                Container.Children.Add(button);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PageSwitcher.Switch(new CategoriesMenu());
        }
    }
}
