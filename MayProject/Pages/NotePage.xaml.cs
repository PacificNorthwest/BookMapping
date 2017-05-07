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

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для NotePage.xaml
    /// </summary>
    public partial class NotePage : UserControl
    {
        private Note _note;
        private IEnumerable<IElement> _notesList;

        public NotePage(IEnumerable<IElement> notesList, IElement note)
        {
            this._note = note as Note;
            this._notesList = notesList;
            InitializeComponent();
            Visualize();
        }

        public NotePage(IEnumerable<IElement> notesList)
        {
            this._notesList = notesList;
            InitializeComponent();
        }

        private void Visualize()
        {
            NoteTitle.Text = _note.Title;
            Note.Text = _note.Text;
        }

        private void SaveNote_Click(object sender, RoutedEventArgs e)
        {
            if (_note == null)
                Bookshelf.Books.Find(book => book.Notes == _notesList).AddNote(NoteTitle.Text, Note.Text);
            else
            {
                _note.Title = NoteTitle.Text;
                _note.Text = Note.Text;
            }
            Bookshelf.Books.Save();
            PageSwitcher.Switch(new ElementMenu(_notesList));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            PageSwitcher.Switch(new ElementMenu(_notesList));
        }
    }
}
