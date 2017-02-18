using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayProject.DataModel;
using MayProject.Contracts;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;

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

        public static void Add(this List<Book> books, string title)
        {
            books.Add(new Book(title));
        }

        public static void Delete(this IElement bookElement)
        {
            var bookElementType = bookElement.GetType().FullName;
            var properties = typeof(Book).GetProperties().Where(property => property.PropertyType.GenericTypeArguments.Length > 0 &&
                                                                property.PropertyType.GenericTypeArguments[0].FullName == bookElementType).ToList();

            foreach (var book in Bookshelf.Books)
            {
                foreach (var property in properties)
                {
                    var list = typeof(Book).GetProperty(property.Name).GetValue(book);
                    list.GetType().GetMethod("Remove").Invoke(list, new object[] { bookElement });
                }
            }
        }

        public static void Delete(this Book book)
        {
            Bookshelf.Books.Remove(book);
        }

        public static void AddIllustration(this IIllustratable illustratableElement, string path)
        {
            illustratableElement.Illustrations.Add(new Bitmap(path));
        }
        public static void DeleteIllustration()
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

        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }
    }
}
