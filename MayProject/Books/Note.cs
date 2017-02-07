using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayProject.Contracts;

namespace MayProject.Books
{
    [Serializable]
    public class Note : IElement
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public Note(string title)
        {
            Title = title;
        }

        public Note() : this("No title")
        { }
    }
}
