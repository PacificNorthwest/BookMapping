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
using System.Windows.Markup;
using MayProject.Controller;

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
            PopulateSideMenu();
        }

        private void PopulateSideMenu()
        {
            //(MainWindow.CurrentItem.DataContext as ScrollViewer).Visibility = Visibility.Visible;
            ((MainWindow.CurrentItem.DataContext as Dictionary<string, object>)["Side menu"] as ScrollViewer)
                                                                               .Visibility = Visibility.Visible;
            var menu = new BookSideMenu();
            menu.SideMenu_Chapters.Children.Clear();
            menu.SideMenu_Characters.Children.Clear();
            menu.SideMenu_Locations.Children.Clear();
            menu.SideMenu_Maps.Children.Clear();
            menu.SideMenu_Notes.Children.Clear();

            foreach (Chapter chapter in _book.Chapters)
            {
                Button plate = new Button();
                plate.Width = plate.Height = 25;
                plate.Margin = new Thickness(2);
                plate.Background = new SolidColorBrush(Color.FromRgb(169, 169, 169));
                plate.Style = menu.FindResource("RoundCorners") as Style;
                plate.Content = _book.Chapters.IndexOf(chapter) + 1;
                plate.Click += (object sender, RoutedEventArgs e) =>
                                PageSwitcher.Switch(new ChapterPage(_book.Chapters,
                                                                    _book.Chapters[Convert.ToInt32(plate.Content) - 1]));
                menu.SideMenu_Chapters.Children.Add(plate);
            }
            foreach (Character character in _book.Characters)
            {
                Button plate = CreateIllustrationPlate(character, menu.FindResource("RoundCorners") as Style);
                plate.Margin = new Thickness(2);
                plate.MaxWidth = 80;
                plate.FontSize = 18;
                plate.Click -= Plate_Click;
                plate.DataContext = character;
                plate.Click += (object sender, RoutedEventArgs e) =>
                                PageSwitcher.Switch(new CharacterProfile(_book.Characters, plate.DataContext as Character));
                menu.SideMenu_Characters.Children.Add(plate);
            }
            foreach (Location location in _book.Locations)
            {
                Button plate = CreateIllustrationPlate(location, menu.FindResource("RoundCorners") as Style);
                plate.Margin = new Thickness(2);
                plate.MaxWidth = 80;
                plate.FontSize = 18;
                plate.Click -= Plate_Click;
                plate.DataContext = location;
                plate.Click += (object sender, RoutedEventArgs e) =>
                                PageSwitcher.Switch(new LocationPage(_book.Locations, plate.DataContext as Location));
                menu.SideMenu_Locations.Children.Add(plate);
            }
            Button relationsMap = new Button();
            relationsMap.Content = "Relations Map";
            relationsMap.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
            relationsMap.FontSize = 18;
            relationsMap.Margin = new Thickness(2);
            relationsMap.Click += (object sender, RoutedEventArgs e) =>
                                  PageSwitcher.Switch(new RelationsMapPage(_book));
            Button eventsMap = new Button();
            eventsMap.Content = "Events Map";
            eventsMap.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
            eventsMap.FontSize = 18;
            eventsMap.Margin = new Thickness(2);
            eventsMap.Click += (object sender, RoutedEventArgs e) =>
                                PageSwitcher.Switch(new EventsMapPage(_book));
            menu.SideMenu_Maps.Children.Add(relationsMap);
            menu.SideMenu_Maps.Children.Add(eventsMap);

            foreach (Note note in _book.Notes)
            {
                Button plate = new Button();
                plate.Background = new SolidColorBrush(Color.FromRgb(128, 128, 128));
                plate.Content = note.Title;
                plate.FontSize = 18;
                plate.DataContext = note;
                plate.Click += (object sender, RoutedEventArgs e) =>
                                PageSwitcher.Switch(new NotePage(_book.Notes, plate.DataContext as Note));
                menu.SideMenu_Notes.Children.Add(plate);
            }
            ((MainWindow.CurrentItem.DataContext as Dictionary<string, object>)["Side menu"] as ScrollViewer).Content = menu;
            //(MainWindow.CurrentItem.DataContext as ScrollViewer).Content = menu;
    }
        private Button CreateIllustrationPlate(IIllustratable element, Style style)
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
            Button illustration = new Button();
            illustration.Style = style;

            Image image = new Image();
            image.Source = img;
            illustration.Content = image;

            Label label = new Label();
            label.VerticalAlignment = VerticalAlignment.Center;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Content = element.Title;

            Button plate = new Button() { Background = new SolidColorBrush(Color.FromArgb(0,0,0,0)),
                                          BorderThickness = new Thickness(0)};
            plate.Content = CreateIllustrationGrid(illustration, label);
            plate.DataContext = element;
            plate.Click += Plate_Click;

            return plate;
        }

        private Grid CreateIllustrationGrid(Button image, Label label)
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
            Grid.SetColumn(viewbox, 0);

            grid.Children.Add(image);
            grid.Children.Add(viewbox);

            return grid;
        }

        private void CategoryClick(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).DataContext as string)
            {
                case "Notes":
                    {
                        PageSwitcher.Switch(new ElementMenu(_book, _book.Notes));
                        break;
                    }
                case "Characters":
                    {
                        PageSwitcher.Switch(new ElementMenu(_book, _book.Characters));
                        break;
                    }
                case "Chapters":
                    {
                        PageSwitcher.Switch(new ElementMenu(_book, _book.Chapters));
                        break;
                    }
                case "Locations":
                    {
                        PageSwitcher.Switch(new ElementMenu(_book, _book.Locations));
                        break;
                    }
                case "Relations":
                    {
                        PageSwitcher.Switch(new RelationsMapPage(_book));
                        break;
                    }
            }
        }

        private void Plate_Click(object sender, RoutedEventArgs e)
        {
            OpenElementPage((sender as Button).DataContext as IElement);
        }

        private void OpenElementPage(IElement element)
        {
            if (element is Book)
                PageSwitcher.Switch(new CategoriesMenu(element as Book));
            if (element is Note)
                PageSwitcher.Switch(new NotePage(_book.Notes, element));
            if (element is Chapter)
                PageSwitcher.Switch(new ChapterPage(_book.Chapters, element));
            if (element is Character)
                PageSwitcher.Switch(new CharacterProfile(_book.Characters, element));
            if (element is Location)
                PageSwitcher.Switch(new LocationPage(_book.Locations, element));
        }
    }
}
