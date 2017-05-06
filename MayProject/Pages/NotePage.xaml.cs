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

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для NotePage.xaml
    /// </summary>
    public partial class NotePage : UserControl
    {
        private Note _note;
        private List<Note> _notesList;

        public NotePage(List<Note> notesList, Note note)
        {
            this._note = note;
            this._notesList = notesList;
            InitializeComponent();
            Visualize();
        }

        public NotePage(List<Note> notesList)
        {
            this._notesList = notesList;
            InitializeComponent();
        }

        private void Visualize()
        {
            NoteTitle.Text = _note.Title;
            Note.Text = _note.Content;
        }

        private void SaveNote_Click(object sender, RoutedEventArgs e)
        {
            if (_note == null)
                _notesList.Add(new DataModel.Note(NoteTitle.Text, Note.Text));
            else
            {
                _note.Title = NoteTitle.Text;
                _note.Content = Note.Text;
            }
            Bookshelf.Books.Save();
            PageSwitcher.Switch(new NotesPage(_notesList));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            PageSwitcher.Switch(new NotesPage(_notesList));
        }
    }
}
