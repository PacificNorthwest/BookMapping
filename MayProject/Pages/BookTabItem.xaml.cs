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

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для BookTabItem.xaml
    /// </summary>
    public partial class BookTabItem : TabItem
    {
        public Stack<UserControl> History { get; set; } = new Stack<UserControl>();
        public UserControl CurrentPage => this.ContentPanel.Children[0] as UserControl;

        public new bool IsSelected
        {
            get { return base.IsSelected; }
            set { base.IsSelected = value; }
        }

        public BookTabItem()
        {
            InitializeComponent();
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            if(History.Count > 0)
                PageSwitcher.Back();
        }
    }
}
