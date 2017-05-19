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
using MayProject.Controller;
using MayProject.Contracts;
using MayProject.Pages;

namespace MayProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Stack<UserControl> History { get; set; } = new Stack<UserControl>();
        public static TabItem CurrentItem => (App.Current.FindResource("WorkArea") as TabControl).SelectedItem as TabItem;
        private TabControl WorkArea { get; set; }

        public MainWindow()
        {
            try
            {
                Bookshelf.Books.Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                InitializeComponent();
                WorkArea = App.Current.FindResource("WorkArea") as TabControl;
                MainContainer.Children.Add(WorkArea);
                PageSwitcher.mainWindow = this;
                this.WorkArea.Items.Add(NewTab());
            }
        }

        private TabItem NewTab()
        {
            TabItem tabItem = new TabItem();
            tabItem.Header = "New tab";
            tabItem.Background = new SolidColorBrush(Colors.DarkGray);
            Grid contentPanel = new Grid();
            contentPanel.Children.Add(new ElementMenu(Bookshelf.Books));

            Button button_back = new Button()
                                     {
                                        VerticalAlignment = VerticalAlignment.Top,
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                        BorderThickness = new Thickness(0),
                                        Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                                        MaxWidth = 20,
                                        Content = new Image() { Source = Properties.Resources.back_arrow.ToBitmapImage() }
                                    };
            button_back.Click += (object sender, RoutedEventArgs e) => PageSwitcher.Back();
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("Side menu", new ScrollViewer()
                                            {
                                                Width = 190,
                                                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                                                Visibility = Visibility.Collapsed
                                            });
            properties.Add("History", new Stack<UserControl>());
            tabItem.DataContext = properties;
            tabItem.Content = TabContent(contentPanel, 
                                         (tabItem.DataContext as Dictionary<string, object>)["Side menu"] as ScrollViewer,
                                         button_back);

            return tabItem;
        }

        public void Navigate(UserControl usercontrol)
        {
            if (usercontrol is CategoriesMenu)
                (this.WorkArea.SelectedItem as TabItem).Header = (usercontrol as CategoriesMenu).BookTitle;

            (((this.WorkArea.SelectedItem as TabItem).Content as Grid).Children[1] as Grid).Children.Clear();
            (((this.WorkArea.SelectedItem as TabItem).Content as Grid).Children[1] as Grid).Children.Add(usercontrol);
        }

        private Grid TabContent(Grid g, ScrollViewer sideMenu, Button button_back)
        {
            Grid grid = new Grid();
            ColumnDefinition sideMenuColumn = new ColumnDefinition();
            ColumnDefinition contentColumn = new ColumnDefinition();
            sideMenuColumn.Width = GridLength.Auto;
            grid.ColumnDefinitions.Add(sideMenuColumn);
            grid.ColumnDefinitions.Add(contentColumn);
            Grid.SetColumn(sideMenu, 0);
            Grid.SetRow(sideMenu, 0);
            Grid.SetColumn(button_back, 1);
            Grid.SetRow(button_back, 0);
            Grid.SetColumn(g, 1);
            Grid.SetRow(g, 0);
            grid.Children.Add(sideMenu);
            grid.Children.Add(g);
            grid.Children.Add(button_back);

            return grid;
        }

        private void NewTabButton_Click(object sender, RoutedEventArgs e)
        {
            this.WorkArea.Items.Add(NewTab());
            this.WorkArea.SelectedIndex = this.WorkArea.Items.Count - 1;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (e.ClickCount == 2)
                {
                    AdjustWindowSize();
                }
                else
                {
                    Application.Current.MainWindow.DragMove();
                }
        }

        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
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
            (((((sender as Button)
                .Parent as Grid)
                .TemplatedParent as ContextMenu)
                .PlacementTarget as Border)
                .DataContext as IElement).Delete();
            Bookshelf.Books.Save();
            (((sender as Button)
                .Parent as Grid)
                .TemplatedParent as ContextMenu).Visibility = Visibility.Collapsed;
        }

        private void ButtonAddIllustration_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ButtonRename_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
