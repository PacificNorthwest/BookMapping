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
    /// Класс главного окна программы
    /// </summary>
    public partial class MainWindow : Window
    {
        public static BookTabItem SelectedTab => (App.Current.FindResource("WorkArea") as TabControl).SelectedItem as BookTabItem;
        private TabControl WorkArea { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public MainWindow()
        {
            try
            {
                Bookshelf.Books.Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                InitializeComponent();
                WorkArea = App.Current.FindResource("WorkArea") as TabControl;
                MainContainer.Children.Add(WorkArea);
                PageSwitcher.MainWindow = this;
                this.WorkArea.Items.Add(NewTab());
            }
        }

        /// <summary>
        /// Фабричный класс для создания новой вкладки
        /// </summary>
        /// <returns>Возвращает экземпляр класса BookTabItem</returns>
        private TabItem NewTab()
        {
            BookTabItem tabItem = new BookTabItem();
            tabItem.Header = "New tab";
            tabItem.Foreground = new SolidColorBrush(Colors.White);
            tabItem.ContentPanel.Children.Add(new ElementMenu(Bookshelf.Books));
            return tabItem;
        }

        /// <summary>
        /// Метод навигации между странвицами приложения
        /// </summary>
        /// <param name="usercontrol">Страница для перехода</param>
        public void Navigate(UserControl usercontrol)
        {
            if (usercontrol is CategoriesMenu)
                SelectedTab.Header = (usercontrol as CategoriesMenu).BookTitle;

            SelectedTab.ContentPanel.Children.Clear();
            SelectedTab.ContentPanel.Children.Add(usercontrol);
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку создания нвоой вкладки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewTabButton_Click(object sender, RoutedEventArgs e)
        {
            this.WorkArea.Items.Add(NewTab());
            this.WorkArea.SelectedIndex = this.WorkArea.Items.Count - 1;
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку закрытия окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Обработчик события нажатия клавишей мыши на верхней панели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Подстривание размера окна
        /// </summary>
        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        /// <summary>
        /// Обработчик события вхождения курсора мыши в область кнопки контекстного меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuButton_MouseOver(object sender, RoutedEventArgs e)
        {
            (sender as Button).Background = new SolidColorBrush(Color.FromArgb(255, 178, 34, 34));
            ((((sender as Button).Parent as Grid).Children
                .Cast<UIElement>()
                .First(x => x.GetType() == typeof(Viewbox)) as Viewbox).Child as TextBlock)
                .Text = (sender as Button).DataContext as string;
        }

        /// <summary>
        /// Обработчик события выхода курсора мыши из области кнопки контекстного меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuButton_MouseLeave(object sender, RoutedEventArgs e)
        {
            (sender as Button).Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            ((((sender as Button).Parent as Grid).Children
                .Cast<UIElement>()
                .First(x => x.GetType() == typeof(Viewbox)) as Viewbox).Child as TextBlock)
                .Text = string.Empty;
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку удаления элемента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Обработчик события нажатия на кнопку добавления иллюстрации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddIllustration_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        //private void ButtonRename_Click(object sender, RoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
