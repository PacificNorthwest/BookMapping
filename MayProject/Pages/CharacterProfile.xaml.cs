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
using MayProject.Controller;
using MayProject.DataModel;

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для CharacterProfile.xaml
    /// </summary>
    public partial class CharacterProfile : UserControl
    {
        private Character _character;
        private IEnumerable<IElement> _charactersList;

        public CharacterProfile(IEnumerable<IElement> charactersList, IElement character)
        {
            _character = character as Character;
            _charactersList = charactersList;
            InitializeComponent();
            Visualize();
        }

        private void Visualize()
        {
            if (_character.Illustrations.Count > 0)
                Illustration.Source = _character.Illustrations[0].ToBitmapImage();
            CharacterName.Text = _character.Title;
            Description.Text = _character.Description;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            _character.Title = CharacterName.Text;
            _character.Description = Description.Text;
            Bookshelf.Books.Save();
            PageSwitcher.Switch(new ElementMenu(_charactersList));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            PageSwitcher.Switch(new ElementMenu(_charactersList));
        }
    }
}
