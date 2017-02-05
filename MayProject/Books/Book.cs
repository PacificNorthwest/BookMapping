using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayProject.Books
{
    [Serializable]
    class Book
    {
        public string BookName { get; set; }
        public List<Character> Characters { get; set; }
        public List<Location> Locations { get; set; }
        public List<Note> Notes { get; set; }
        public List<Chapter> Chapters { get; set; }

        public Book(string bookName)
        {
            BookName = bookName;
            Characters = new List<Character>();
            Locations = new List<Location>();
            Notes = new List<Note>();
            Chapters = new List<Chapter>();
        }

    }
}
