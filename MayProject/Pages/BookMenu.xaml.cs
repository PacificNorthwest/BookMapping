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
        public BookMenu()
        {
            Bookshelf.Books.Load();
            InitializeComponent();
            Visualize();
        }

        private void Visualize()
        {
            StringBuilder buttonXaml = new StringBuilder();
            buttonXaml.Append(@"<Button xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' 
                                        xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' ");
            buttonXaml.Append("Margin = '20' FontSize = '30' MaxWidth = '200'>");
            buttonXaml.Append("</Button>");

            foreach (Book book in Bookshelf.Books)
            {
                //Тест
                book.Illustrations.Add(Properties.Resources.book_05);

                BitmapImage img = book.Illustrations[0].ToBitmapImage();
                Button button = XamlReader.Parse(buttonXaml.ToString()) as Button;

                Image image = new Image();
                image.Source = img;

                Label label = new Label();
                label.VerticalAlignment = VerticalAlignment.Center;
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.Content = book.Title;

                button.DataContext = book;
                button.Content = CreateGrid(image, label);
                button.Click += Button_Click;
                Container.Children.Add(button);
            }
        }

        private Grid CreateGrid(Image image, Label label)
        {
            Grid grid = new Grid();
            Viewbox viewbox = new Viewbox();
            viewbox.Child = label;
            RowDefinition firstRow = new RowDefinition();
            firstRow.Height = GridLength.Auto;
            RowDefinition secondRow = new RowDefinition();
            secondRow.Height = new GridLength(0.3, GridUnitType.Star);
            grid.RowDefinitions.Add(firstRow);
            grid.RowDefinitions.Add(secondRow);
            Grid.SetRow(image, 0);
            Grid.SetColumn(image, 0);
            Grid.SetRow(viewbox, 1);
            Grid.SetColumn(viewbox, 1);

            grid.Children.Add(image);
            grid.Children.Add(viewbox);

            return grid;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PageSwitcher.Switch(new CategoriesMenu((sender as Button).DataContext as Book));
        }
    }
}
