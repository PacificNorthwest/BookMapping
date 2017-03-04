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

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для NewBookWindow.xaml
    /// </summary>
    public partial class NewBookWindow : Window
    {
        public string NewBookTitle { get; private set; }

        public NewBookWindow()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxBookTitle.Text == string.Empty)
                MessageBox.Show("Empty title!");
            else
                NewBookTitle = textBoxBookTitle.Text;
            this.Close();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            textBoxBookTitle.Text = string.Empty;
            this.Close();
        }
    }
}
