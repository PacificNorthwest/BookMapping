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
using MayProject.Contracts;
using System.Reflection;
using System.ComponentModel;
using Microsoft.Win32;

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для BookMenu.xaml
    /// </summary>
    public partial class ElementMenu : UserControl, ISideMenuHandler
    {
        private IEnumerable<IElement> _elements;
        private IElement _focusedElement;
        private Book _book;

        public ElementMenu(IEnumerable<IElement> elements)
        {
            _elements = elements;
            InitializeComponent();
            Visualize();
        }

        public ElementMenu(Book book, IEnumerable<IElement> elements)
        {
            _elements = elements;
            _book = book;
            InitializeComponent();
            Visualize();
        }

        private void Visualize()
        {
            if (_elements.GetType().GenericTypeArguments[0].Name != "Book")
            {
                MainWindow.SelectedTab.SideMenu.Visibility = Visibility.Visible;
                PopulateSideMenu();
            }
            Container.Children.Clear();
            foreach (IElement element in _elements)
            {
                if (element is IIllustratable)
                    Container.Children.Add(CreateIllustrationPlate(element as IIllustratable));
                else
                    Container.Children.Add(CreateTextPlate(element as IPlainTextElement));
            }
            Container.Children.Add(CreateNewElementPlate());
        }

        public void PopulateSideMenu()
        {
            if (MainWindow.SelectedTab.History.Count > 0)
            {
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
                                    PageSwitcher.Switch(new ChapterPage(_book.Chapters, chapter));
                    menu.SideMenu_Chapters.Children.Add(plate);
                }
                foreach (Character character in _book.Characters)
                {
                    Button plate = CreateIllustrationPlate(character, menu.FindResource("RoundCorners") as Style);
                    plate.Margin = new Thickness(2);
                    plate.MaxWidth = 80;
                    plate.FontSize = 18;
                    plate.Click += (object sender, RoutedEventArgs e) => OpenElementPage(character);
                    menu.SideMenu_Characters.Children.Add(plate);
                }
                foreach (Location location in _book.Locations)
                {
                    Button plate = CreateIllustrationPlate(location, menu.FindResource("RoundCorners") as Style);
                    plate.Margin = new Thickness(2);
                    plate.MaxWidth = 80;
                    plate.FontSize = 18;
                    plate.Click += (object sender, RoutedEventArgs e) => OpenElementPage(location);
                    menu.SideMenu_Locations.Children.Add(plate);
                }
                Button relationsMap = new Button();
                relationsMap.Content = "Relations Map";
                relationsMap.Background = new SolidColorBrush(Color.FromArgb(255, 236, 235, 231));
                relationsMap.FontSize = 18;
                relationsMap.Margin = new Thickness(2);
                relationsMap.Click += (object sender, RoutedEventArgs e) =>
                                      PageSwitcher.Switch(new RelationsMapPage(_book));
                Button eventsMap = new Button();
                eventsMap.Content = "Events Map";
                eventsMap.Background = new SolidColorBrush(Color.FromArgb(255, 236, 235, 231));
                eventsMap.FontSize = 18;
                eventsMap.Margin = new Thickness(2);
                eventsMap.Click += (object sender, RoutedEventArgs e) =>
                                    PageSwitcher.Switch(new EventsMapPage(_book));
                menu.SideMenu_Maps.Children.Add(relationsMap);
                menu.SideMenu_Maps.Children.Add(eventsMap);

                foreach (Note note in _book.Notes)
                {
                    Button plate = new Button();
                    plate.Background = new SolidColorBrush(Color.FromArgb(255, 236, 235, 231));
                    plate.Content = note.Title;
                    plate.FontSize = 18;
                    plate.DataContext = note;
                    plate.Click += (object sender, RoutedEventArgs e) =>
                                    PageSwitcher.Switch(new NotePage(_book.Notes, note));
                    menu.SideMenu_Notes.Children.Add(plate);
                }

                MainWindow.SelectedTab.SideMenu.Content = menu;
            }
            else
               MainWindow.SelectedTab.SideMenu.Visibility = System.Windows.Visibility.Collapsed;
        }

        private Button CreateIllustrationPlate(IIllustratable element)
        {
            ImageSource img;
            if (element.Illustrations.Count > 0)
                img = element.Illustrations[element.Illustrations.Count - 1].ToBitmapImage();
            else
            {
                if (element is Book)
                    img = Properties.Resources.defaultIllustration.ToBitmapImage();
                else if (element is Character)
                    img = Properties.Resources.avatar.ToBitmapImage();
                else if (element is Location)
                    img = Properties.Resources.park.ToBitmapImage();
                else
                    img = Properties.Resources.defaultIllustration.ToBitmapImage();
            }
            Button plate = CreatePlateTemplate();

            Image image = new Image();
            image.Source = img;
            image.Stretch = Stretch.UniformToFill;

            Label label = new Label();
            label.VerticalAlignment = VerticalAlignment.Center;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Content = element.Title;

            plate.Content = CreateIllustrationGrid(image, label);
            plate.Click += (object sender, RoutedEventArgs e) => OpenElementPage(element);
            plate.MouseRightButtonDown += (object sender, MouseButtonEventArgs e) 
                                            => _focusedElement = element;

            return plate;
        }

        private Button CreateTextPlate(IPlainTextElement element)
        {
            Button plate = CreatePlateTemplate();

            Label label = new Label();
            label.FontSize = 30;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Margin = new Thickness(5, 5, 0, 0);
            label.Content = element.Title;

            TextBlock textBlock = new TextBlock();
            textBlock.FontSize = 25;
            textBlock.Margin = new Thickness(5);
            textBlock.Text = element.Text;
            textBlock.TextWrapping = TextWrapping.Wrap;
            StackPanel panel = new StackPanel();
            panel.Children.Add(textBlock);

            plate.Content = CreateTextGrid(label, panel);
            plate.Click += (object sender, RoutedEventArgs e) => OpenElementPage(element);
            plate.MouseRightButtonDown += (object sender, MouseButtonEventArgs e)
                                            => _focusedElement = element;

            return plate;
        }

        private Button CreateNewElementPlate()
        {
            Button plate = CreatePlateTemplate();
            Image newElementIcon = new Image();
            newElementIcon.Source = Properties.Resources.plus.ToBitmapImage();
            plate.Content = newElementIcon;
            plate.Click += (object sender, RoutedEventArgs e) => { CreateNewElement(); };
            return plate;
        }

        private Button CreatePlateTemplate()
        {
            StringBuilder buttonXaml = new StringBuilder();
            buttonXaml.Append(@"<Button xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' 
                                        xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' ");
            buttonXaml.Append("Margin = '20' FontSize = '30' MaxWidth = '200'>");
            buttonXaml.Append("</Button>");

            return XamlReader.Parse(buttonXaml.ToString()) as Button;
        }
        
        private Grid CreateIllustrationGrid(Image image, Label label)
        {
            Grid grid = new Grid();
            Viewbox viewbox = new Viewbox();
            viewbox.MaxHeight = 60;
            viewbox.Child = label;
            RowDefinition firstRow = new RowDefinition();
            firstRow.Height = new GridLength(0.7, GridUnitType.Star);
            RowDefinition secondRow = new RowDefinition();
            secondRow.Height = GridLength.Auto;
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

        private Grid CreateTextGrid(Label label, StackPanel note)
        {
            Grid grid = new Grid();
            RowDefinition firstRow = new RowDefinition();
            firstRow.Height = GridLength.Auto;
            RowDefinition secondRow = new RowDefinition();
            secondRow.Height = new GridLength(1, GridUnitType.Star);
            grid.RowDefinitions.Add(firstRow);
            grid.RowDefinitions.Add(secondRow);
            Grid.SetRow(label, 0);
            Grid.SetColumn(label, 0);
            Grid.SetRow(note, 1);
            Grid.SetColumn(note, 1);

            grid.Children.Add(label);
            grid.Children.Add(note);

            return grid;
        }

        private Button CreateIllustrationPlate(IIllustratable element, Style style)
        {
            ImageSource img;
            if (element.Illustrations.Count > 0)
                img = element.Illustrations[element.Illustrations.Count - 1].ToBitmapImage();
            else
            {
                if (element is Character)
                    img = ((byte[])new System.Drawing.ImageConverter().ConvertTo(Properties.Resources.character, typeof(byte[]))).ToBitmapImage();
                else if (element is Location)
                    img = ((byte[])new System.Drawing.ImageConverter().ConvertTo(Properties.Resources.location, typeof(byte[]))).ToBitmapImage();
                else
                    img = Properties.Resources.defaultIllustration.ToBitmapImage();
            }
            Button illustration = new Button();
            illustration.Style = style;

            Image image = new Image();
            image.Source = img;
            
            image.Stretch = Stretch.UniformToFill;
            illustration.Content = image;

            Label label = new Label();
            label.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            label.FontSize = 25;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Content = element.Title;

            Button plate = new Button()
            {
                Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                BorderThickness = new Thickness(0)
            };
            plate.Content = CreateIllustrationGrid(illustration, label);
            plate.Click += (object sender, RoutedEventArgs e) => OpenElementPage(element);

            return plate;
        }

        private Grid CreateIllustrationGrid(Button image, Label label)
        {
            Grid grid = new Grid();
            Viewbox viewbox = new Viewbox();
            viewbox.MaxHeight = 20;
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

        private void OpenElementPage(IElement element)
        {
                if (element is Book)
                PageSwitcher.Switch(new CategoriesMenu(element as Book));
            if (element is Note)
                PageSwitcher.Switch(new NotePage(_elements, element));
            if (element is Chapter)
                PageSwitcher.Switch(new ChapterPage(_elements, element));
            if (element is Character)
                PageSwitcher.Switch(new CharacterProfile(_elements, element));
            if (element is Location)
                PageSwitcher.Switch(new LocationPage(_elements, element));
        }

        private void CreateNewElement()
        {
            switch (_elements.GetType().GetGenericArguments()[0].Name.ToString())
            {
                case "Book":
                    {
                        CreateNewBook();
                        break;
                    }
                case "Note":
                    {
                        CreateNewNote();
                        break;
                    }
                case "Character":
                    {
                        CreateNewCharacter();
                        break;
                    }
                case "Chapter":
                    {
                        CreateNewChapter();
                        break;
                    }
                case "Location":
                    {
                        CreateNewLocation();
                        break;
                    }
            }
        }

        private void CreateNewBook()
        {
            NewElementWindow newBookWindow = new NewElementWindow();
            newBookWindow.ShowDialog();
            string newBookTitle = newBookWindow.NewElementTitle;
            if (!string.IsNullOrEmpty(newBookTitle))
            {
                Bookshelf.Books.Add(newBookTitle);
                Bookshelf.Books.Save();
            }
            Visualize();
        }

        private void CreateNewNote()
        {
            PageSwitcher.Switch(new NotePage(_elements));
        }

        private void CreateNewCharacter()
        {
            NewElementWindow newCharacterWindow = new NewElementWindow();
            newCharacterWindow.ShowDialog();
            string newCharacterName = newCharacterWindow.NewElementTitle;
            if (!string.IsNullOrEmpty(newCharacterName))
            {
                Bookshelf.Books.Find(book => book.Characters == _elements).AddCharacter(newCharacterName);
                Bookshelf.Books.Save();
            }
            Visualize();
        }

        private void CreateNewChapter()
        {
            NewElementWindow newChapterWindow = new NewElementWindow();
            newChapterWindow.ShowDialog();
            string newChapterTitle = newChapterWindow.NewElementTitle;
            if (!string.IsNullOrEmpty(newChapterTitle))
            {
                Bookshelf.Books.Find(book => book.Chapters == _elements).AddChapter(newChapterTitle);
                Bookshelf.Books.Save();
            }
            Visualize();
        }

        private void CreateNewLocation()
        {
            NewElementWindow newLocationWindow = new NewElementWindow();
            newLocationWindow.ShowDialog();
            string newLocationTitle = newLocationWindow.NewElementTitle;
            if (!string.IsNullOrEmpty(newLocationTitle))
            {
                Bookshelf.Books.Find(book => book.Locations == _elements).AddLocation(newLocationTitle);
                Bookshelf.Books.Save();
            }
            Visualize();
        }

        private void ContextMenuButton_MouseOver(object sender, RoutedEventArgs e)
        { 
            (sender as Button).Background = new SolidColorBrush(Color.FromArgb(255, 178, 34, 34));
            ((((sender as Button).Parent as Grid).Children
                .Cast<UIElement>()
                .First(x => x.GetType() == typeof(Viewbox)) as Viewbox).Child as TextBlock)
                .Text = (sender as Button).DataContext as string;
        }

        private void ContextMenuButton_MouseLeave(object sender, RoutedEventArgs e)
        {
            (sender as Button).Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            ((((sender as Button).Parent as Grid).Children
                .Cast<UIElement>()
                .First(x => x.GetType() == typeof(Viewbox)) as Viewbox).Child as TextBlock)
                .Text = string.Empty;
        }

        private void ButtonDeleteElement_Click(object sender, RoutedEventArgs e)
        {
            _focusedElement.Delete();
            Bookshelf.Books.Save();

            (((sender as Button)
                .Parent as Grid)
                .TemplatedParent as ContextMenu).Visibility = Visibility.Collapsed;
            Visualize();
        }

        private void ButtonAddIllustration_Click(object sender, RoutedEventArgs e)
        {
            if (_focusedElement is IIllustratable)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                    (_focusedElement as IIllustratable).AddIllustration(dialog.FileName);
                Bookshelf.Books.Save();
                Visualize();
            }
        }

        private void ButtonRename_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
