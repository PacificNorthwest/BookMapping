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
using System.Windows.Media;

namespace MayProject.Controller
{
    /// <summary>
    /// Контроллер, содержащий расширяющие методы для работы с коллекциями и экземплярами классов модели.
    /// </summary>
    static class BookControl
    {
        /// <summary>
        /// Сохранение модели данных в файл
        /// </summary>
        /// <param name="books">Список книг</param>
        public static void Save(this List<Book> books)
        {
            XmlManager.Save(books);
        }

        /// <summary>
        /// Загрузка данных из файла
        /// </summary>
        /// <param name="books">Список книг</param>
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

        /// <summary>
        /// Добавление нвоой книги в коллекцию книг
        /// </summary>
        /// <param name="books">Список книг</param>
        /// <param name="title">Название книги</param>
        public static void Add(this List<Book> books, string title)
        {
            books.Add(new Book(title));
        }

        /// <summary>
        /// Удаление элемента из модели данных
        /// </summary>
        /// <param name="bookElement">Элемент для удаления</param>
        public static void Delete(this IElement bookElement)
        {
            var bookElementType = bookElement.GetType().FullName;
            var properties = typeof(Book).GetProperties().Where(property => property.PropertyType.GenericTypeArguments.Length > 0 &&
                                                                property.PropertyType.GenericTypeArguments[0].FullName == bookElementType).ToList();

            foreach (var book in Bookshelf.Books)
                foreach (var property in properties)
                {
                    var list = typeof(Book).GetProperty(property.Name).GetValue(book);
                    list.GetType().GetMethod("Remove").Invoke(list, new object[] { bookElement });
                }
        }

        /// <summary>
        /// Удаление книги из списка книг
        /// </summary>
        /// <param name="book">Книга для удаления</param>
        public static void Delete(this Book book)
        {
            Bookshelf.Books.Remove(book);
        }

        /// <summary>
        /// Добавление иллюстрации к элементу
        /// </summary>
        /// <param name="illustratableElement">Элемент для добавления иллюстрации</param>
        /// <param name="path">Путь к изображению в файловой системе</param>
        public static void AddIllustration(this IIllustratable illustratableElement, string path)
        {
            illustratableElement.Illustrations.Add(File.ReadAllBytes(path));
        }

        /// <summary>
        /// Добавление персонажа в книгу
        /// </summary>
        /// <param name="book">Книга для добавления</param>
        /// <param name="name">Имя персонажа</param>
        public static void AddCharacter(this Book book, string name)
        {
            book.Characters.Add(new Character(name));
        }

        /// <summary>
        /// Добавление главы в книгу
        /// </summary>
        /// <param name="book">Книга для добавления</param>
        /// <param name="title">Название главы</param>
        public static void AddChapter(this Book book, string title)
        {
            book.Chapters.Add(new Chapter(title));
        }

        /// <summary>
        /// Добавление локации в книгу
        /// </summary>
        /// <param name="book">Книга для добавления</param>
        /// <param name="title">Название локации</param>
        public static void AddLocation(this Book book, string title)
        {
            book.Locations.Add(new Location(title));
        }

        /// <summary>
        /// Добавление заметки в книгу
        /// </summary>
        /// <param name="book">Книга для добавления</param>
        /// <param name="title">Название заметки</param>
        /// <param name="content">Текст заметки</param>
        public static void AddNote(this Book book, string title, string content)
        {
            book.Notes.Add(new Note(title, content));
        }

        /// <summary>
        /// Конвертация Bitmap в BitmapImage
        /// </summary>
        /// <param name="bitmap">Bitmap для конвертации</param>
        /// <returns></returns>
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

        /// <summary>
        /// Конвертаци byte[] в ImageSource
        /// </summary>
        /// <param name="bytes">Массив байтов, представляющих изображение</param>
        /// <returns></returns>
        public static ImageSource ToBitmapImage(this byte[] bytes) => 
                     (ImageSource)new ImageSourceConverter().ConvertFrom(bytes);
       
    }
}
