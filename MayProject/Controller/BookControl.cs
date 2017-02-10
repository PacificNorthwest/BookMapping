using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayProject.DataModel;
using MayProject.Contracts;
using System.Drawing;

namespace MayProject.Controller
{
    static class BookControl
    {

        public static void Save(this List<Book> books)
        {
            XmlManager.Save(books);
        }

        public static void Load(this List<Book> books)
        {
            Bookshelf.Books = XmlManager.Load(books.GetType()) as List<Book>;
        }

        public static void AddBook(this List<Book> books, string title)
        {
            books.Add(new Book(title));
        }

        public static void Delete(this IElement bookElement)
        {
            var bookElementType = bookElement.GetType().FullName;
            var propertys = typeof(Book).GetProperties().Where(x => x.PropertyType.GenericTypeArguments.Length > 0 &&
                                                               x.PropertyType.GenericTypeArguments[0].FullName == bookElementType).ToList();
            foreach (var property in propertys)
            {
                var list = typeof(Book).GetProperty(property.Name).GetValue(Bookshelf.Books[0]);
                list.GetType().GetMethod("Remove").Invoke(list, new object[] { bookElement });
            }
        }

        public static void AddIllustration(this IIllustratable bookElement, string path)
        {
            bookElement.Illustrations.Add(new Bitmap(path));
        }
        public static void DeleteIllustration(this IIllustratable bookElement, int ID)
        {
            throw new NotImplementedException();
        }

        public static void AddCharacter(this Book book, string name)
        {
            book.Characters.Add(new Character(name));
        }

        public static void AddChapter(this Book book, string title)
        {
            book.Chapters.Add(new Chapter(title));
        }

        public static void AddLocation(this Book book, string title)
        {
            book.Locations.Add(new Location(title));
        }

        public static void AddNote(this Book book, string title)
        {
            book.Notes.Add(new Note(title));
        }
    }
}
