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
using System.Windows.Shapes;
using MayProject.DataModel;
using MayProject.Contracts;
using MayProject.Controller;
using System.Windows.Markup;

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для NewEventWindow.xaml
    /// </summary>
    public partial class NewEventWindow : Window
    {
        private Book _book;

        public NewEventWindow(Book book)
        {
            _book = book;
            InitializeComponent();
            Visualize();
        }

        private void Visualize()
        {
            CharactersList.Items.Clear();
            LocationsList.Items.Clear();
            foreach (Character character in _book.Characters)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = CreateIllustrationPlate(character);
                CharactersList.Items.Add(item);
            }
            foreach (Location location in _book.Locations)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = CreateIllustrationPlate(location);
                LocationsList.Items.Add(item);
            }
        }

        private Grid CreateIllustrationPlate(IIllustratable element)
        {
            BitmapImage img;
            if (element.Illustrations.Count > 0)
                img = element.Illustrations[element.Illustrations.Count - 1].ToBitmapImage();
            else
            {
                if (element is Character)
                    img = Properties.Resources.avatar.ToBitmapImage();
                else if (element is Location)
                    img = Properties.Resources.park.ToBitmapImage();
                else
                    img = Properties.Resources.defaultIllustration.ToBitmapImage();
            }
            Image image = new Image();
            image.Source = img;

            Label label = new Label();
            label.VerticalAlignment = VerticalAlignment.Center;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Content = element.Title;

            Grid grid = CreateIllustrationGrid(image, label);
            grid.DataContext = element.Title;
            return grid;
        }

        private Grid CreatePlateTemplate()
        {
            StringBuilder buttonXaml = new StringBuilder();
            buttonXaml.Append(@"<Grid xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' 
                                        xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' ");
            buttonXaml.Append("Margin = '20' MaxWidth = '100' MaxHeight = '150'>");
            buttonXaml.Append("</Grid>");

            return XamlReader.Parse(buttonXaml.ToString()) as Grid;
        }

        private Grid CreateIllustrationGrid(Image image, Label label)
        {
            Grid grid = CreatePlateTemplate();
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

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            this.Close();
        }

        private void ClearFields()
        {
            EventTitle.Text = string.Empty;
            ChapterName.Text = string.Empty;
            EventDescription.Text = string.Empty;
        }

        private void BottonOK_Click(object sender, RoutedEventArgs e)
        {
            List<Character> characters = new List<Character>();
            foreach (ListBoxItem item in CharactersList.SelectedItems)
                characters.Add(_book.Characters.Find(character => character.Title == (item.Content as Grid).DataContext as string));
            Location location = _book.Locations.Find(loc => loc.Title == (((LocationsList.SelectedItem as ListBoxItem)
                                                                            .Content as Grid)
                                                                            .DataContext as string));
            _book.Events.Add(new Event(EventTitle.Text,
                                       EventDescription.Text,
                                       ChapterName.Text,
                                       characters,
                                       location));
            Bookshelf.Books.Save();
            ClearFields();
            this.Close();
        }
    }
}
