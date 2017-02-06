using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayProject.Books
{
    [Serializable]
    public class Book : AbstractIllustratableElement
    {
        public List<Character> Characters { get; set; } = new List<Character>();
        public List<Location> Locations { get; set; } = new List<Location>();
        public List<Note> Notes { get; set; } = new List<Note>();
        public List<Chapter> Chapters { get; set; } = new List<Chapter>();

        public Book(string title)
        {
            Title = title;
            Illustrations = new List<System.Drawing.Bitmap>();
        }

        public Book() : this("No title")
        { }

        public override void AddIllustration(string path)
        {
            throw new NotImplementedException();
        }

        public override void RemoveIllustration()
        {
            throw new NotImplementedException();
        }

    }
}
