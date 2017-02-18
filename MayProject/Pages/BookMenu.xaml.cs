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
using System.IO;
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
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public BookMenu()
        {
            InitializeComponent();
            Bookshelf.Books.Load();
            Visualize();
        }

        private void Visualize()
        {
            //Тест
            for (int i = 0; i < 20; i++)
            {
                Bookshelf.Books.Add(i.ToString());
            }

            foreach (Book book in Bookshelf.Books)
            {
                //Тест
                book.Illustrations.Add(Properties.Resources.book_05);

                BitmapImage img = book.Illustrations[0].ToBitmapImage();
                StringBuilder buttonXaml = new StringBuilder();
                buttonXaml.Append(@"<Button xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' 
                                            xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' ");
                buttonXaml.Append("Margin = '20' FontSize = '30' MaxWidth = '200'>");
                buttonXaml.Append("</Button>");

                //Допилить контент
                Button button = XamlReader.Parse(buttonXaml.ToString()) as Button;

                Image image = new Image();
                image.Source = img;

                Grid grid = new Grid();
                grid.Children.Add(image);

                Label label = new Label();
                label.Content = book.Title;
                label.Margin = new Thickness(30, 120, 0, 0);
                grid.Children.Add(label);

                button.DataContext = book;
                button.Content = grid;

                button.Click += Button_Click;
                Container.Children.Add(button);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PageSwitcher.Switch(new CategoriesMenu((sender as Button).DataContext as Book));
        }
    }
}
