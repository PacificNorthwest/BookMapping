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
        public static ScrollViewer SideMenu { get; set; } =
                                    new ScrollViewer() { Width = 190,
                                                         VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                                                         Visibility = Visibility.Collapsed };
        public static Grid ContentPanel { get; set; } = new Grid();
        private static Grid _tabContent;

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
                _tabContent = TabContent(ContentPanel);
                PageSwitcher.mainWindow = this;
                this.WorkArea.Items.Add(NewTab());
            }
        }

        private TabItem NewTab()
        {
            TabItem tabItem = new TabItem();
            tabItem.Header = "New tab";
            tabItem.Background = new SolidColorBrush(Colors.DarkGray);
            ContentPanel.Children.Clear();
            ContentPanel.Children.Add(new ElementMenu(Bookshelf.Books));
            tabItem.Content = _tabContent;

            return tabItem;
        }

        public void Navigate(UserControl usercontrol)
        {
            if (usercontrol is CategoriesMenu)
                (this.WorkArea.SelectedItem as TabItem).Header = (usercontrol as CategoriesMenu).BookTitle;
            ContentPanel.Children.Clear();
            ContentPanel.Children.Add(usercontrol);
            (this.WorkArea.SelectedItem as TabItem).Content = _tabContent;
        }

        private Grid TabContent(Grid g)
        {
            Grid grid = new Grid();
            ColumnDefinition sideMenuColumn = new ColumnDefinition();
            ColumnDefinition contentColumn = new ColumnDefinition();
            sideMenuColumn.Width = GridLength.Auto;
            grid.ColumnDefinitions.Add(sideMenuColumn);
            grid.ColumnDefinitions.Add(contentColumn);
            Grid.SetColumn(SideMenu, 0);
            Grid.SetRow(SideMenu, 0);
            Grid.SetColumn(g, 1);
            Grid.SetRow(g, 0);
            grid.Children.Add(SideMenu);
            grid.Children.Add(g);

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
