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
    /// Логика взаимодействия для ChapterPage.xaml
    /// </summary>
    public partial class ChapterPage : UserControl
    {
        private IEnumerable<IElement> _chapterList;
        private Chapter _chapter;

        public ChapterPage(IEnumerable<IElement> elements, IElement chapter)
        {
            _chapterList = elements;
            _chapter = chapter as Chapter;
            InitializeComponent();
            Visualize();
        }

        private void Visualize()
        {
            ChapterTitle.Text = _chapter.Title;
            ChapterDescription.Text = _chapter.Annotation;
            ChapterContent.Text = _chapter.Text;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            _chapter.Title = ChapterTitle.Text;
            _chapter.Annotation = ChapterDescription.Text;
            _chapter.Text = ChapterContent.Text;
            Bookshelf.Books.Save();
            PageSwitcher.Switch(new ElementMenu(_chapterList));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            PageSwitcher.Switch(new ElementMenu(_chapterList));
        }
    }
}
