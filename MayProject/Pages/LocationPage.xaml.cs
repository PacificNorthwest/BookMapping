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
using MayProject.Contracts;
using MayProject.DataModel;
using MayProject.Controller;

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для LocationPage.xaml
    /// </summary>
    public partial class LocationPage : UserControl
    {
        private IEnumerable<IElement> _locationsList;
        private Location _location;

        public LocationPage(IEnumerable<IElement> locationsList, IElement location)
        {
            _location = location as Location;
            _locationsList = locationsList;
            InitializeComponent();
            Visualize();
        }

        private void Visualize()
        {
            if (_location.Illustrations.Count > 0)
                Illustration.Source = _location.Illustrations[0].ToBitmapImage();
            LocationTitle.Text = _location.Title;
            Description.Text = _location.Description;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            PageSwitcher.Switch(new ElementMenu(_locationsList));
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            _location.Title = LocationTitle.Text;
            _location.Description = Description.Text;
            Bookshelf.Books.Save();
            PageSwitcher.Switch(new ElementMenu(_locationsList));
        }
    }
}
