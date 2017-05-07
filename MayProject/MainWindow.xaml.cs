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
        public MainWindow()
        {
            Bookshelf.Books.Load();
            InitializeComponent();
            PageSwitcher.mainWindow = this;
            this.WorkArea.Items.Add(NewTab());
        }

        private TabItem NewTab()
        {
            TabItem tabItem = new TabItem();
            tabItem.Header = "New tab";
            tabItem.Background = new SolidColorBrush(Colors.DarkGray);
            tabItem.Content = new ElementMenu(Bookshelf.Books);

            return tabItem;
        }

        public void Navigate(UserControl usercontrol)
        {
            if (usercontrol is CategoriesMenu)
                (this.WorkArea.SelectedItem as TabItem).Header = (usercontrol as CategoriesMenu).BookTitle;
            (this.WorkArea.SelectedItem as TabItem).Content = usercontrol;
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

    }
}
