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
using System.Windows.Markup;
using MayProject.Controller;

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для NotesPage.xaml
    /// </summary>
    public partial class NotesPage : UserControl
    {
        private List<Note> _notes;
        public NotesPage(List<Note> notes)
        {
            this._notes = notes;
            InitializeComponent();
            Visualize();
        }

        private void Visualize()
        {
            Container.Children.Clear();
            StringBuilder buttonXaml = new StringBuilder();
            buttonXaml.Append(@"<Button xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' 
                                        xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' ");
            buttonXaml.Append("Margin = '20' MaxWidth = '200'>");
            buttonXaml.Append("</Button>");

            foreach (Note note in _notes)
            {
                Label label = new Label();
                label.FontSize = 30;
                label.VerticalAlignment = VerticalAlignment.Top;
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.Margin = new Thickness(5, 5, 0, 0);
                label.Content = note.Title;

                TextBlock textBlock = new TextBlock();
                textBlock.FontSize = 25;
                textBlock.Margin = new Thickness(5);
                textBlock.Text = note.Content;
                textBlock.TextWrapping = TextWrapping.Wrap;
                StackPanel panel = new StackPanel();
                panel.Children.Add(textBlock);
                Grid grid = CreateGrid(label, panel);

                Button noteButton = XamlReader.Parse(buttonXaml.ToString()) as Button;
                noteButton.Content = grid;
                noteButton.Click += (object sender, RoutedEventArgs e) => { PageSwitcher.Switch(new NotePage(_notes, note)); };
                Container.Children.Add(noteButton);
            }

            Button buttonNewNote = XamlReader.Parse(buttonXaml.ToString()) as Button;
            Image newNoteIcon = new Image();
            newNoteIcon.Source = Properties.Resources.plus.ToBitmapImage();
            buttonNewNote.Content = newNoteIcon;
            buttonNewNote.Click += (object sender, RoutedEventArgs e) => { PageSwitcher.Switch(new NotePage(_notes)); };
            Container.Children.Add(buttonNewNote);
        }

        private Grid CreateGrid(Label label, StackPanel note)
        {
            Grid grid = new Grid();
            RowDefinition firstRow = new RowDefinition();
            firstRow.Height = GridLength.Auto;
            RowDefinition secondRow = new RowDefinition();
            secondRow.Height = new GridLength(1, GridUnitType.Star);
            grid.RowDefinitions.Add(firstRow);
            grid.RowDefinitions.Add(secondRow);
            Grid.SetRow(label, 0);
            Grid.SetColumn(label, 0);
            Grid.SetRow(note, 1);
            Grid.SetColumn(note, 1);

            grid.Children.Add(label);
            grid.Children.Add(note);

            return grid;
        }
    }
}
