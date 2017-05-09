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
            try
            {
                Bookshelf.Books = XmlManager.Load(books.GetType()) as List<Book>;
            }
            catch(Exception ex)
            {
                Bookshelf.Books = new List<Book>();
                throw new Exception("Failed to load Books list. New one created.");
            }
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

        public static void AddNote(this Book book, string title, string content)
        {
            book.Notes.Add(new Note(title, content));
        }

        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            MemoryStream memory = new MemoryStream();
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
            memory.Position = 0;
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = memory;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();

            return img;
        }
    }
}
